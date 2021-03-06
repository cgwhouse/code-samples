# A program to help facilitate the activities of movie club
# Cristian Widenhouse
#
from sqlite3 import connect
from termcolor import colored
import colorama

queue_sel = 'SELECT title FROM Movies WHERE picker_ID = ? AND watched = 0;'
queue_ins = 'INSERT INTO Movies VALUES (NULL, ?, ?, 0);'
queue_del = 'DELETE FROM Movies WHERE title = ?;'
queue_watch = 'UPDATE Movies SET watched = 1 WHERE title = ?;'
watched_list = 'SELECT title, name FROM Movies INNER JOIN Members ON picker_ID = member_ID WHERE watched = 1;'
club_members = 'SELECT name FROM Members;'
member_ID_from_name = 'SELECT member_ID FROM Members WHERE name = ?;'
conn = connect('movie_club.db')
c = conn.cursor()


def view_movie_queues():
    member_queues = []
    names = member_list()
    for name in names:
        member_queues.append(create_queue(name))
    for queue in member_queues:
        print(queue)
    present_actions()


def add_to_queue():
    names = member_list()
    queue_prompt = '\nWhich queue do you want to add to? Options are: '
    for i in range(len(names)):
        queue_prompt += names[i]
        if i != len(names) - 1:
            queue_prompt += ', '
    print(queue_prompt)
    selection = input()
    while selection not in names:
        print(colored('\nInvalid queue selection.', 'red'))
        print(queue_prompt)
        selection = input()
    print(f"\nGreat! We will use {selection}'s queue to add movies.")
    c.execute(member_ID_from_name, (selection,))
    member_ID = c.fetchone()[0]
    movies_to_add = []
    movie_prompt = '\nEnter the name of a movie to add (press enter to exit):'
    print(movie_prompt)
    movie = input()
    while movie != '':
        movies_to_add.append(movie)
        print(movie_prompt)
        movie = input()
    for movie in movies_to_add:
        c.execute(queue_ins, (movie, member_ID))
    print(colored('\nMovies have been added successfully.', 'green'))
    conn.commit()
    present_actions()


def edit_list(movie_prompt, error_string, sql_query, success_msg,
              update_picker=False):
    movies = []
    print(movie_prompt)
    movie = input()
    while movie != '':
        movies.append(movie)
        print(movie_prompt)
        movie = input()
    for movie in movies:
        c.execute('SELECT movie_ID FROM Movies WHERE title = ?;', (movie,))
        test = c.fetchone()
        if test is None:
            print(
                colored(f'Error: {movie} is not a title {error_string}', 'red'))
        else:
            c.execute(sql_query, (movie,))
            if update_picker:
                update_the_picker()
    print(colored(
        f'\nMovies have been {success_msg} successfully (bad inputs were ignored).', 'green'))
    conn.commit()
    present_actions()


def delete_from_queue():
    movie_prompt = '\nEnter the name of a movie to delete (press enter to exit):'
    error_string = 'in the queue.'
    success_msg = 'deleted'
    edit_list(movie_prompt, error_string, queue_del, success_msg)


def add_to_watched():
    movie_prompt = '\nEnter the name of a movie you watched (press enter to exit):'
    error_string = 'in the queue.'
    success_msg = 'added to "watched" list'
    edit_list(movie_prompt, error_string, queue_watch, success_msg, True)


def delete_from_watched():
    movie_prompt = '\nEnter the name of a movie to delete (press enter to exit):'
    error_string = 'on the list.'
    success_msg = 'deleted'
    edit_list(movie_prompt, error_string, queue_del, success_msg)


def view_watched():
    c.execute(watched_list)
    watched = c.fetchall()
    result = '\n\nList of Watched Movies:\n-----------------------\n'
    for movie in watched:
        result += f'{movie[0]} added by {movie[1]}\n'
    print(result.rstrip())
    present_actions()


def view_club_members():
    c.execute(club_members)
    members = c.fetchall()
    result = '\n\nList of Club Members:\n---------------------\n'
    for member in members:
        result += member[0] + '\n'
    print(result.rstrip())
    present_actions()


def add_club_member():
    name = input('\nEnter the name of a member to add (press enter to exit): ')
    if name:
        c.execute('INSERT INTO Members VALUES (NULL, ?);', (name,))
        print(
            colored(f'\n{name} has been successfully added to the club. Welcome!', 'green'))
        conn.commit()
    present_actions()


def remove_club_member():
    print(colored("\nWARNING: Removing a club member will also wipe all of his/her data from the club's records (i.e. all queue and 'watched movie' data).", 'red'))
    choice = input('\nAre you sure you want to proceed? (y/n): ')
    if choice.lower() == 'y':
        name = input(
            '\nEnter the name of the club member you wish to remove (press enter to exit): ')
        if name:
            c.execute(member_ID_from_name, (name,))
            try:
                member_ID = c.fetchone()[0]
            except TypeError:
                print(
                    colored(f'\nError: {name} is not the name of a current club member.', 'red'))
            else:
                c.execute('DELETE FROM Movies WHERE picker_ID = ?;',
                          (member_ID,))
                c.execute('DELETE FROM Members WHERE member_ID = ?;',
                          (member_ID,))
                conn.commit()
                print(colored(
                    f'\n{name} was successfully removed from the club. Good riddance.', 'green'))
    present_actions()


def member_list():
    c.execute('SELECT name FROM Members;')
    names = c.fetchall()
    actual = []
    for name in names:
        actual.append(name[0])
    return actual


def create_queue(name):
    result = f"\n\n{name}'s Queue:\n-----------------------------\n"
    c.execute(member_ID_from_name, (name,))
    member_id = c.fetchone()[0]
    c.execute(queue_sel, (member_id,))
    movies = c.fetchall()
    for movie in movies:
        result += movie[0] + '\n'
    return result.rstrip()


def show_club_status():
    c.execute('SELECT * FROM ProgramInfo;')
    member_ID = c.fetchone()[0]
    c.execute('SELECT name FROM Members WHERE member_ID = ?;', (member_ID,))
    current_picker = c.fetchone()[0]
    print(
        f'\n\nThe person selecting the next movie is {current_picker}.\n\nWould you like to override this?')
    answer = input('y/n: ')
    if answer == 'y':
        manual_update_the_picker()
    present_actions()


def manual_update_the_picker():
    names = member_list()
    result = '\n\nWho do you want to be the new movie selector? Options are: '
    for i in range(len(names)):
        result += names[i]
        if i != len(names) - 1:
            result += ', '
    result += '\n'
    print(result)
    new_selector = input('Name: ')
    while new_selector not in names:
        print(colored(f'\n{new_selector} is not a member of the club.', 'red'))
        print(result)
        new_selector = input('Name: ')
    c.execute(member_ID_from_name, (new_selector,))
    member_ID = c.fetchone()[0]
    c.execute('UPDATE ProgramInfo SET current_selector = ?;', (member_ID,))
    print(colored('\nThe current selector has been updated successfully.', 'green'))


def update_the_picker():
    c.execute('SELECT count(*) FROM Members;')
    m_count = c.fetchone()[0]
    c.execute('SELECT current_selector FROM ProgramInfo;')
    curr_id = c.fetchone()[0]
    if curr_id + 1 > m_count:
        c.execute('UPDATE ProgramInfo SET current_selector = ?;', (1,))
    else:
        c.execute(
            'UPDATE ProgramInfo SET current_selector = ?;', (curr_id + 1,))
    conn.commit()


def goodbye():
    conn.commit()
    conn.close()
    print(colored('\nGoodbye!\n', 'yellow'))


action_dict = {1: view_movie_queues, 2: add_to_queue, 3: delete_from_queue, 4: view_watched, 5: add_to_watched,
               6: delete_from_watched, 7: view_club_members, 8: add_club_member, 9: remove_club_member, 10: show_club_status, 11: goodbye}


def get_action(name):
    action_dict.get(name, lambda: 'Invalid')()


def present_actions():
    actions = ['View Movie Queues', 'Add to a Queue', 'Delete from a Queue',
               'View Watched List', 'Add to Watched List',
               'Delete from Watched List', 'View Club Members', 'Add a Club Member',
               'Remove a Club Member', 'Whose Turn Is It?', 'Exit the Program']
    menu = '\n\nSelect an Action:\n-----------------\n'
    n = 1
    for action in actions:
        menu += f'{n} - {action}\n\n'
        n += 1
    print(menu)
    try:
        action = int(input('Action: '))
        if action not in range(1, n):
            raise ValueError
        get_action(action)
    except ValueError:
        print(colored('\n\nInvalid Action. Try Again.\n\n', 'red'))
        present_actions()


def main():
    # Enables ANSI colors on Windows
    colorama.init()

    with open('greeting_string.txt') as file:
        greeting = file.read()
    print('\n\n\n\n\n' + colored(greeting, 'cyan'))
    present_actions()


main()
