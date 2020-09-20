# Release Retention

Features:
-	NET Core 3.1 API solving the release retention problem
-	Some unit tests with more mocked data
-	Basic MVC UI 

TODO:
-	Extend unit testing with more mocked data scenarios and simplify the testing assertions

Future Ideas:
- 	Return early after finding the last N releases so it doesn't continue iterating deployments
- 	Could rewrite many operations using LINQ instead
- 	Cater for failed deployments (maybe exclude these)
- 	Restrict retrieval of recent releases to a date range so releases from inactive projects aren't returned
- 	Allow for different N configurations per project/environment (e.g. maybe keep less releases for dev/staging)
- 	Extend to cater for release packages/files