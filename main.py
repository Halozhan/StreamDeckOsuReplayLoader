"""
reference: https://frhyme.github.io/python-libs/python_read_recent_file_in_python/
"""

import os

path = "F:/osu!/Replays/"
file_list = os.listdir(path)

print(f"리플레이: {len(file_list)}개")

file_name_and_time_list = []

for f_name in os.listdir(path):
    written_time = os.path.getctime(f"{path}{f_name}")
    file_name_and_time_list.append((f_name, written_time))

sorted_file_list = sorted(file_name_and_time_list, key=lambda x: x[1], reverse=True)

recent_file = sorted_file_list[0]
recent_file_name = recent_file[0]

print(recent_file_name)
os.startfile(f"{path}{recent_file_name}")