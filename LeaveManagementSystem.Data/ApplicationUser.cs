﻿namespace LeaveManagementSystem.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }

        //date of joining the company
        public DateOnly DateOfJoining { get; set; }
    }
}
