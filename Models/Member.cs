using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraDesk.Models
{
    /// <summary>
    /// Xin, Anagha,Elijah and Aryan, Aug 20,2025
    /// object class for Member made according to the database structure
    /// </summary>
    public class Member
    {
        int memberId;
        string memberName;

        
        public int MemberId { get => memberId; set => memberId = value; }
        public string MemberName { get => memberName; set => memberName = value; }
        public Member()
        {
        }

        public Member(int memberId, string memberName)
        {
            this.MemberId = memberId;
            this.MemberName = memberName;
        }

        public override string ToString()
        {
            return $"ID: {MemberId}, Name: {MemberName}";
        }
    }
}

