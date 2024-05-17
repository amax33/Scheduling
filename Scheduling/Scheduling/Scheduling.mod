// Parameters (define these parameters before the sets)
int nc = ...; // number of classes
int ns = ...; // number of subjects
int nts = ...; // number of teachers
int np = ...; // number of periods per day
int nd = ...; // number of days per week

// Define ranges
range C = 1..nc; // range of Classes in a school
range S = 1..ns; // range of Subjects in a school
range Ts = 1..nts; // range of Teachers for each subject s in a school
range P = 1..np; // range of number of periods in each day
range D = 1..nd; // range of number of working days in each week

// Parameters
int ht[s in S][t in Ts]; // Define the number of periods that teacher t must teach subject s in the school
int hc[c in C][s in S]; // Define the number of periods that subject s must be studied in class c
int hcTotal[c in C]; // Define all periods that must be taught in class c

// Define the tuples for periods
tuple Period {
  int p;
  int d;
}
{Period} At[s in S][t in Ts]; // Define the set of periods that teacher t can teach subject s in school
{Period} Bt[s in S][t in Ts]; // Define the set of periods that teacher t can but doesn't like to teach subject s in school

// Decision variables
dvar boolean Yc[c in C][t in Ts][s in S]; // teacher t assign to class c for subject s
dvar boolean Xc[c in C][t in Ts][s in S][p in P][d in D]; // teacher t teaches subject s to class c in period p in day d

// Auxiliary variables
dvar boolean OverloadedZ3[t in Ts][s in S][d in D][c in C];
dvar boolean ImbalancedZ4[t in Ts][s in S][d in D][c in C];
dvar boolean ImbalancedZ5[t in Ts][s in S][d in D];
dvar boolean LastPeriodZ6[t in Ts][s in S][d in D][c in C];

// Objective function
dexpr int Z1[t in Ts][s in S] = sum(<p, d> in Bt[s][t], c in C) Xc[c][t][s][p][d]; // unlike time of teacher t
dexpr int Z2[t in Ts][s in S] = sum(d in D) (np - sum(p in P, c in C) Xc[c][t][s][p][d] - 1); // each subject should teach once in a day
dexpr int Z3[t in Ts][s in S] = sum(d in D, c in C) OverloadedZ3[t][s][d][c];
dexpr int Z4[t in Ts][s in S] = sum(d in D, c in C) ImbalancedZ4[t][s][d][c];
dexpr int Z5[t in Ts][s in S] = sum(d in D) ImbalancedZ5[t][s][d];
dexpr int Z6[t in Ts][s in S] = sum(d in D, c in C) LastPeriodZ6[t][s][d][c];

// Weights (define these somewhere in your data or parameter section)
float W1 = ...;
float W2 = ...;
float W3 = ...;
float W4 = ...;
float W5 = ...;
float W6 = ...;

minimize sum(t in Ts, s in S) (W1 * Z1[t][s] + W2 * Z2[t][s] + W3 * Z3[t][s] + W4 * Z4[t][s] + W5 * Z5[t][s] + W6 * Z6[t][s]);

// Hard constraints
subject to {
  // 1. No teacher can teach more than one class in a period of a day
  forall(t in Ts, s in S, p in P, d in D)
    sum(c in C) Xc[c][t][s][p][d] <= 1;
  
  // 2. One class can't have two teachers in a period of a day
  forall(c in C, p in P, d in D)
    sum(s in S, t in Ts) Xc[c][t][s][p][d] <= 1;
  
  // 3. Total periods of teaching from teacher t in subject s to class c
  forall(s in S, t in Ts)
    sum(c in C, d in D, p in P) Xc[c][t][s][p][d] == ht[s][t];
  
  // 4. Periods of teaching from teacher t in subject s to class c
  forall(c in C, t in Ts, s in S)
    sum(d in D, p in P) Xc[c][t][s][p][d] >= hc[c][s] * Yc[c][t][s];
  
  // 5. Each class must have hc periods in week
  forall(c in C)
    sum(s in S, t in Ts, d in D, p in P) Xc[c][t][s][p][d] == hcTotal[c];
  
  // 6. Teacher t that assign to subject s, they must not have class in time he is not in school
  forall(t in Ts, s in S, p in P, d in D)
    if (!(<p,d> in At[s][t])) 
      forall(c in C) Xc[c][t][s][p][d] == 0;
  
  // 7. Classes must be in continuous periods, only possible if the break is in the last period of day
  forall(c in C, d in D, p in P : p < np)
    sum(s in S, t in Ts) Xc[c][t][s][p + 1][d] - sum(s in S, t in Ts) Xc[c][t][s][p][d] >= 0;
  
  // 8. First period must not be empty
  forall(c in C, d in D)
    sum(s in S, t in Ts) Xc[c][t][s][1][d] == 1;
    
  // Auxiliary constraints to handle operator issues
  // Constraint for Z3: if a teacher has 2 periods of s in one day
  forall(t in Ts, s in S, d in D, c in C)
    OverloadedZ3[t][s][d][c] == (sum(p in P) Xc[c][t][s][p][d] >= 2);

  // Constraint for Z4: normalize a schedule
  forall(t in Ts, s in S, d in D, c in C)
    ImbalancedZ4[t][s][d][c] == (sum(p in P) Xc[c][t][s][p][d] - (1.0 / nd) * hc[c][s] * Yc[c][t][s] >= 1);

  // Constraint for Z5: normalize the teacher schedule
  forall(t in Ts, s in S, d in D)
    ImbalancedZ5[t][s][d] == (sum(p in P, c in C) Xc[c][t][s][p][d] - (1.0 / nd) * ht[s][t] >= 1);

  // Constraint for Z6: reducing amount of last periods for a teacher
  forall(t in Ts, s in S, d in D, c in C)
    LastPeriodZ6[t][s][d][c] == Xc[c][t][s][np][d];
}
