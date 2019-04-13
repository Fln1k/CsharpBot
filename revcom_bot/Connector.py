import sys
import sqlite3
import logging
import telebot


class SQL:
	path_to_sql = None
	conn = None
	cursor = None

	def __init__(self, path):
		self.path_to_sql = path
		self.conn = sqlite3.connect(self.path_to_sql)
		self.cursor = self.conn.cursor()

	def search(self,request):
		self.cursor.execute(r"SELECT * FROM Request WHERE Request IS NOT NULL")
		data = self.cursor.fetchall()
		pointer = 0
		max_coincidence = 0
		search_index = 0
		request = request.split(" ")
		"""
		print(pointer)
		print(request)
		"""
		try:
			while True:
				coincidence = 0
				for word in request:
					if data[pointer][0].count(word):
						coincidence+=1
				"""print("Check: " + data[pointer][0]+ " coincidence: "+str(coincidence))"""
				if max_coincidence<coincidence:
					max_coincidence = coincidence
					search_index = pointer
				pointer+=1
		except:
			"""print("Max coincidence: " + data[search_index][0])"""
			if max_coincidence == 0:
				return("don't uderstand")
			return data[search_index][1]

sql = SQL(r"C:\Users\Fln1k\Desktop\revcom_bot\revcom_bot\bin\Debug\Database.db")
pointer = 1
request = ""
while True:
	request += sys.argv[pointer]
	if pointer+1 < len(sys.argv):
		pointer += 1
		request += " "
	else:
		break
answer = sql.search(request)
print(answer)