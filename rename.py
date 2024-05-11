import os

rename_list = [
    'GameTemplate',
    'Template',
]

ignored_dirs = [
    '.godot',
    '.git'
]

ignored_files = [
    'rename.py',

    'steam_api.dll',
    'steam_api.lib',
    'steam_api64.dll',
    'steam_api64.lib',
    'libsteam_api.bundle',
    'libsteam_api.so',
]

def replace_in_file(file_path, old_text, new_text):
    try:
        with open(file_path, 'r') as file:
            filedata = file.read()
        
        filedata = filedata.replace(old_text, new_text)
        with open(file_path, 'w') as file:
            file.write(filedata)
    except Exception as e:
        print(f"Encountered error with file '{file_path}': {e}")

def rename_folders(root_dir, old_text, new_text):
    for root, dirs, files in os.walk(root_dir):
        dirs[:] = [d for d in dirs if d not in ignored_dirs]
        files[:] = [d for d in files if d not in ignored_files]


        for directory in dirs:
            new_directory_name = directory.replace(old_text, new_text)
            os.rename(os.path.join(root, directory), os.path.join(root, new_directory_name))


def rename_files(root_dir, old_text, new_text):
    for root, dirs, files in os.walk(root_dir):
        dirs[:] = [d for d in dirs if d not in ignored_dirs]
        files[:] = [d for d in files if d not in ignored_files]


        for file_name in files:
            if old_text in file_name:
                new_file_name = file_name.replace(old_text, new_text)
                file_path = os.path.join(root, file_name)
                new_file_path = os.path.join(root, new_file_name)
                os.rename(file_path, new_file_path)


def rename_file_contents(root_dir, old_text, new_text):
    for root, dirs, files in os.walk(root_dir):
        dirs[:] = [d for d in dirs if d not in ignored_dirs]
        files[:] = [d for d in files if d not in ignored_files]

        for file_name in files:
            file_path = os.path.join(root, file_name)
            replace_in_file(file_path, old_text, new_text)

def main():
    root_dir = '.'  # Change this to the root directory where renaming should start
    new_text = input('Enter the new name: ')  # Prompt user for the new name

    for ren in rename_list:
        rename_folders(root_dir, ren, new_text)
        rename_files(root_dir, ren, new_text)
        rename_file_contents(root_dir, ren, new_text)

    print('Renaming complete.')

if __name__ == '__main__':
    main()