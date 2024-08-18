# School Timetable Scheduling Project

## Overview

This project focuses on automating the scheduling of timetables for schools using optimization techniques. It addresses the complexities of balancing teacher availability, class requirements, and subject periods, providing an optimized solution that adheres to given constraints. The project is implemented using **C#** with the **IBM ILOG CPLEX** optimization library and an **OPL (Optimization Programming Language)** model.

### Features
- Optimizes class schedules based on teacher availability, subject periods, and class constraints.
- Supports customizable constraints to meet school-specific requirements.
- Utilizes **IBM ILOG CPLEX** for robust optimization.
- Includes a console-based application and an OPL model.

## Project Structure

├── ConsoleApp1/ # Console application implemented in C#

│ ├── Program.cs # Main program logic

│ ├── Class.cs # Class-related logic

│ ├── Days.cs # Days-related logic

│ ├── Subject.cs # Subject-related logic

│ ├── Teacher.cs # Teacher-related logic

└── Scheduling/ # Folder containing the OPL model

├── Scheduling.mod # OPL model with decision variables and constraints


## Technologies Used

- **C#**
- **IBM ILOG CPLEX**: Optimization engine
- **OPL**: Optimization Programming Language



## Example Output

Solution status = Optimal

Objective value = 3

Class 1:

        Monday:
        
                8-9 - Literature - teacher1
                
                9-10 - Chemistry - teacher1

                10-11 - Math - teacher3
                
        Tuesday:
        
                8-9 - Chemistry - teacher1
                
                9-10 - Math - teacher3
                
                10-11 - Physics - teacher1
                
        Wednesday:
        
                8-9 - Physics - teacher1
                
                9-10 - Literature - teacher1
                
                10-11 - Math - teacher3
                
Class 2:

        Monday:
        
                8-9 - Physics - teacher3
                
                9-10 - Math - teacher2
                
                10-11 - Literature - teacher2
                
        Tuesday:
        
                8-9 - Chemistry - teacher4
                
                9-10 - Math - teacher2
                
                10-11 - Physics - teacher3
                
        Wednesday:
        
                8-9 - Literature - teacher2
                
                9-10 - Math - teacher2
                
                10-11 - Chemistry - teacher4
                


### Feel free to adapt the code to read data from csv files...
