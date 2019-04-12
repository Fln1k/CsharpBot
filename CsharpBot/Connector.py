import sys
import sqlite3
import logging


class SQL:
	path_to_sql = None
	conn = None
	cursor = None

	def __init__(self, path):
		self.path_to_sql = path
		self.conn = sqlite3.connect(self.path_to_sql)
		self.cursor = self.conn.cursor()

	def create_table(self):
		self.cursor.execute("""CREATE TABLE ReqAnsw
								(Request text, Answer text);
								""")
		logging.info(self.cursor)

	def add_data(self, _request, answer):
		data_to_add = list()
		data_to_add.append(_request)
		data_to_add.append(answer)
		logging.info(self.cursor)
		self.cursor.execute("INSERT INTO ReqAnsw(Request, Answer) VALUES (?,?);", (_request, answer,))
		self.conn.commit()


sql = SQL("Database.db")
if sys.argv[1] == "Request":
	pointer = 2
	request = ""
	while True:
		request += sys.argv[pointer]
		if pointer+1 < len(sys.argv):
			pointer += 1
			request += " "
		else:
			break
	print("done")
if sys.argv[1] == "AddData":
	try:
		sql.create_table()
	except:
		pass
	pointer = 2
	request = ""
	while True:
		request += sys.argv[pointer]
		if pointer + 1 < len(sys.argv):
			pointer += 1
			request += " "
		else:
			break
	request=request.split("|")
	sql.add_data(request[0], request[1])
	print("Added")

