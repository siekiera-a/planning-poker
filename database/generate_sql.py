from glob import glob
from sys import argv
from os.path import join

def copy_files(output_file, directory):
  print(f'adding {directory}')
  for file in glob(join(start_directory, directory, '**', '*.sql'), recursive=True):
    with open(file, 'r') as input_file:
      output_file.write(f'-- from: {file}\n')
      output_file.write(input_file.read())
      output_file.write('\n')

if __name__ == "__main__":
  if (len(argv) != 3):
    print(f"Usage: {__file__} [start_directory] [output_file]")
    exit(1)

  start_directory = argv[1]
  output_file = argv[2]

  with open(output_file, 'w') as out_file:
    # add views
    copy_files(out_file, 'views')

    # add functions
    copy_files(out_file, 'functions')

    # add procedures
    copy_files(out_file, 'procedures')

    
    

