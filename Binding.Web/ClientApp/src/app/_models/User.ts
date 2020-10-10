import {Page, PageWithNoBlocksViewModel} from "./Page";

export interface User
{
  Id: string;
  DisplayName: string;
  Email: string;
  Password: string;
  Token: string;
  Created: Date;
  Updated: Date;
  Pages: Page[];
}

export interface UserViewModel
{
  Id: string;
  DisplayName: string;
  Email: string;
  Password: string;
  Token: string;
  Created: Date;
  Updated: Date;
  Pages: PageWithNoBlocksViewModel[];
}

// public class UserViewModel
// {
//   public Guid Id { get; set; }
// public string DisplayName { get; set; }
// public string Email { get; set; }
// public string Password { get; set; }
// public string Token { get; set; }
// public DateTime Created { get; set; }
// public DateTime Updated { get; set; }
// public IList<PageViewModel> Pages { get; set; }
// }
