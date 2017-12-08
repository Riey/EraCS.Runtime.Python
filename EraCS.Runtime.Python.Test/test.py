def system_title():
	con.PrintLine("Hello world!");
	var.Time = 1000

def ERA_INIT(program):
	global era
	global con
	global var

	era = program
	con = era.Console
	var = era.VarData

def ERA_START():
	system_title()
