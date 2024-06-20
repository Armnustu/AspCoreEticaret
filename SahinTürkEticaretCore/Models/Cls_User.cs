using Microsoft.EntityFrameworkCore;
using SahinTürkEticaretCore.Controllers;
using System.Text;
using XSystem.Security.Cryptography;

namespace SahinTürkEticaretCore.Models
{
	public class Cls_User
	{
		DataContext context = new DataContext();
		public async Task<bool> Login(User user)
		{
			string md5Sifre = MD5Sifrele(user.Password);
			User? user1 = await context.users.Where(x => x.Email == user.Email && x.Password == md5Sifre).FirstOrDefaultAsync();
			return user1 != null ? true : false;

		}
		public static async Task<string> MemberControl(User user)
		{
			using (DataContext context = new DataContext())
			{
				string answer = "";

				try
				{
					string md5Sifre = MD5Sifrele(user.Password);
					User? usr = await context.users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == md5Sifre);

					if (usr == null)
					{
						//kullanıcı yanlıs sifre veya emal girdi
						answer = "error";
					}
					else
					{
						//kullanıcı veritabanında kayıtlı.
						if (usr.IsAdmin == true)
						{
							//admin yetkisi olan personel giriş yapıyor
							answer = "admin";
						}
						else
						{
							answer = usr.Email;
						}
					}
				}
				catch (Exception)
				{
					return "HATA";
				}
				return answer;
			}
		}
		public static string MD5Sifrele(string value)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] btr = Encoding.UTF8.GetBytes(value);
			btr = md5.ComputeHash(btr);

			StringBuilder sb = new StringBuilder();

			foreach (byte item in btr)
			{
				sb.Append(item.ToString("x2").ToLower());
			}
			return sb.ToString();
		}
		public static async Task<User?> SelectMemberInfo(string email)
		{
			using (DataContext context = new DataContext())
			{
				User? user = await context.users.FirstOrDefaultAsync(u => u.Email == email);
				return user;
			}
		}
		public static async Task<bool> LoginEmailControl(User user)
		{
			using (DataContext context = new DataContext())
			{
				User usr = await context.users.FirstOrDefaultAsync(x => x.Email == user.Email);
				if (usr == null)
				{
					return false;
				}
				return true;
			}

		}
		public static async Task<bool> AddUser(User user)
		{
			using (DataContext context = new DataContext())
			{
				try
				{

					user.IsAdmin = false;
					user.Password = MD5Sifrele(user.Password);
					context.users.Add(user);
					context.SaveChanges();
					return true;

				}
				catch (Exception)
				{
					return false;

				}
			}
		}
	}
}
