# Taxes

a small application which manages taxes applied in different municipalities.
The taxes are scheduled in time. Application should provide the user an ability to get taxes applied in
certain municipality at the given day.
Example: Municipality Vilnius has its taxes scheduled like this :
- yearly tax = 0.2 (for period 2016.01.01-2016.12.31),
- monthly tax = 0.4 (for period 2016.05.01-2016.05.31),
- it has no weekly taxes scheduled,
- and it has two daily taxes scheduled = 0.1 (at days 2016.01.01 and 2016.12.25).
The result according to provided example would be:

Municipality (Input)  Date (Input)  Result
Vilnius  				2016.01.01  0.1
Vilnius  				2016.05.02  0.4
Vilnius  				2016.07.10  0.2
Vilnius  				2016.03.16  0.2