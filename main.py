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
        # hour = [(8, 10),(10, 12), ...]


class Teacher:
    def __init__(self, degree, expr, days):
        self.degree = degree
        self.experience = expr
        self.days = days


class Class:
    def __init__(self, grade, days, number):
        self.grade = grade
        self.number = number
        self.days = days

    def get_class_days_name(self):
        days = []
        for day in self.days:
            days.append(day.name)
        return days


class School:
    def __init__(self, subjects, days, teachers, classes):
        self.subjects = subjects
        self.days = days
        self.teachers = teachers
        self.classes = classes
        self.schedule = self.initialize_schedule()

    def initialize_schedule(self):
        schedule = {}
        for i,classes in enumerate(self.classes):
            schedule[i] = classes
            for j,day in enumerate(self.days):
                schedule[i][j] = day
                for h in enumerate(day.hours):
                    schedule[classes.number][day.name][h] = None
        return schedule

    def assign_class(self, day, hour, class_obj, subject_obj, teacher_obj):
        if self.schedule[class_obj.number][day.name][hour] is None:
            self.schedule[class_obj.number][day.name][hour] = (subject_obj, teacher_obj)
            return True
        return False

    def check_same_teacher_for_a_subject_in_one_class(self, class_obj, teacher_obj, subject_obj):
        return True

    def display_schedule(self):
        for day, hours in self.schedule.items():
            print(day + ":")
            for hour, assignment in hours.items():
                print(
                    f"\t{hour} - {assignment[0].name if assignment else 'Free'} - {assignment[1].name if assignment else 'Any'}")

    def get_class_numbers(self):
        numbers = []
        for clas in self.classes:
            numbers.append(clas.number)
        return numbers






def validator(solution):
    '''

    sunday: 1 - math - najaf -
    :param solution:
    :return:
    '''


def main():
    print("")


if __name__ == "__main__":
    main()