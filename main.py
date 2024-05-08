class Subject:
    def __init__(self, name, general, time):
        self.name = name
        self.generality = general
        self.time = time


class Day:
    def __init__(self, name, certain, hour):
        self.name = name
        self.certainty = certain
        self.hour = hour


class Teacher:
    def __init__(self, degree, expr, days):
        self.degree = degree
        self.experience = expr
        self.days = days


class Class:
    def __init__(self, grade, days):
        self.grade = grade
        self.days = days


def main():
    print("")


if __name__ == "__main__":
    main()
