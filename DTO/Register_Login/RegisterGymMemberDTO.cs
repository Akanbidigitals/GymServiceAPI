﻿namespace GymMembershipAPI.DTO.GymMember
{
    public class RegisterGymMemberDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Guid GymOwnerId {  get; set; }


    }
}
