using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraDesk.Models
{ /// <summary>
  /// Xin, Anagha,Elijah and Aryan, Aug 20,2025
  /// manager class for Member to handle operations related to members
  /// </summary>
    public class MemberManager
    {
        static DBhandler db = new DBhandler();

        public static List<Member> members = new List<Member>();

        public static List<Member> SearchMember(string memberName) //Search for members by name
        {
            DBhandler db = new DBhandler();
            return db.SearchMembers(memberName);
        }

        public static string AddMember(Member member) //Add a new member to the database
        {
            // Insert member into the database and get the new ID
            int newMemberId = db.AddMember(member); // DBhandler should return the inserted MemberId
            member.MemberId = newMemberId;          // Update member object with generated ID
            return $"Member '{member.MemberName}' added successfully! ID: {newMemberId}";
        }

        public static string EditMember(Member member) //Edit an existing member's details
        {
            db.EditMember(member);
            return $"Member ID {member.MemberId} updated successfully!";
        }

        public static string RemoveMember(int memberId) //Remove a member from the database
        {
            db.RemoveMember(memberId);
            return $"Member ID {memberId} removed successfully!";
        }
    }
}
