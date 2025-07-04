SELECT TOP (1000) [Id]
      ,[StartDate]
      ,[EndDate]
      ,[LeaveTypeId]
      ,[RequestStatusId]
      ,[EmployeeId]
      ,[ReviewerId]
      ,[RequestComment]
  FROM [LeaveManagementSystemDb].[dbo].[LeaveRequests]



-- Query to get the total number of leave requests per employee
-- along with their names, ordered by the total requests in descending order

SELECT EmployeeId AS EmployeeId
    ,COUNT(*) AS TotalRequests,
    AspNetUsers.FirstName AS EmployeeName,
    AspNetUsers.LastName AS EmployeeLastName    
FROM [LeaveManagementSystemDb].[dbo].[LeaveRequests]    
JOIN [LeaveManagementSystemDb].[dbo].[AspNetUsers] ON LeaveRequests.EmployeeId = AspNetUsers.Id
GROUP BY EmployeeId, AspNetUsers.FirstName, AspNetUsers.LastName
ORDER BY TotalRequests DESC;

SELECT TOP (1000) [Id]
      ,[Name]
      ,[NumberOfDays]
  FROM [LeaveManagementSystemDb].[dbo].[LeaveTypes]
  WHERE Id IN (
      SELECT DISTINCT LeaveTypeId
      FROM [LeaveManagementSystemDb].[dbo].[LeaveRequests]
  );