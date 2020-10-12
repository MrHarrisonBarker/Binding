using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Binding.Test.Seeds
{
    public class BindingContextSeed
    {
        public static async Task SeedAsync(BindingContext bindingContext)
        {
            var userId = Guid.Parse("c385730a-91a5-429f-9c0c-b3e9bd4e5788");
            var pageId = Guid.Parse("b1af5bdb-3771-4c17-baa4-346814bb6ecc");
            var blockId = Guid.Parse("1ed89919-95f0-4a0d-8352-f71a39fb32dc");
            var blockWithNoPageId = Guid.Parse("786871F4-D35B-481F-89A5-92ECA56CC650");
            var emptyPageId = Guid.Parse("08d86a58-8c6e-4a0c-8af8-8dee9e95ed66");
            var pageWithBlocksId = Guid.Parse("08d86a58-8cde-4f61-8831-ae7d5f8705e6");
            var pageWithChildrenId = Guid.Parse("AB7DFBF6-0EFD-4425-AC7F-84239116AB95");
            var pageWithChildrenChildrenId = new Guid("BDDDBEF0-9955-4247-B36C-244919C10129");
            var childPageId = Guid.NewGuid();

            var block = new Block()
            {
                Type = BlockType.Heading,
                Content = "Hello world",
                Id = blockId
            };
            
            var blockWithNoPage = new Block()
            {
                Type = BlockType.Heading,
                Content = "Hello world",
                Id = blockWithNoPageId
            };

            var childPage = new Page()
            {
                Id = childPageId,
                Name = "This is a child page"
            };

            var page = new Page()
            {
                Id = pageId,
                Name = "New Page",
                Blocks = new List<Block>() {block},
                Children = new List<Page>() {childPage}
            };
            
            var pageWithChildren = new Page()
            {
                Id = pageWithChildrenId,
                Name = "Page with children",
                Children = new List<Page>()
                {
                    new Page()
                    {
                        Id = new Guid("7074922C-0F9E-4383-8291-ED3A0A1C3006"),
                        Name = "Child 1"
                    },
                    new Page()
                    {
                        Id = new Guid("80FC8B21-1816-4648-87E4-E8052056A823"),
                        Name = "Child 1"
                    }
                }
            };
            
            var pageWithChildrenChildren = new Page
            {
                Id = pageWithChildrenChildrenId,
                Name = "Page with children",
                Children = new List<Page>()
                {
                    new Page()
                    {
                        Id = new Guid("25496AFA-C143-48F7-B287-CEC262AFD973"),
                        Name = "Child 1",
                        Children = new List<Page>()
                        {
                            new Page()
                            {
                                Name = "Child of child 1",
                                Id = new Guid("76B3E804-BC8C-4310-9A22-243249D34F94")
                            }
                        }
                    }
                }
            };

            var pageWithBlocks = new Page()
            {
                Id = pageWithBlocksId,
                Name = "Page with blocks",
                Blocks = new List<Block>()
                {
                    new Block
                    {
                        Id = Guid.Parse("A13E6844-84C1-44AD-8C29-B60CC542934D"),
                        Type = BlockType.Heading,
                        Content = "Hello world",
                        Order = 0
                    },
                    new Block
                    {
                        Id = Guid.Parse("D9E10FFE-1022-48F1-8593-9F1B29A97C33"),
                        Type = BlockType.Text,
                        Content = "Hello world but as text",
                        Order = 1
                    }
                }
            };

            var emptyPage = new Page()
            {
                Id = emptyPageId,
                Name = "Empty Page"
            };

            var user = new User()
            {
                Email = "harrison@thebarkers.me.uk",
                Id = userId,
                Password = "ANP5SII1yYTgrIH8dTPn74nFNi2rVZPgE8neNWyn/6iuKrZ1twCi90s8W0mlv7PrxA==",
                DisplayName = "Harrison Barker",
                Pages = new List<Page>() {page, childPage}
            };
            // password

            await bindingContext.Users.AddAsync(user);
            await bindingContext.Pages.AddAsync(page);
            await bindingContext.Pages.AddAsync(pageWithChildren);
            await bindingContext.Blocks.AddAsync(block);
            await bindingContext.Blocks.AddAsync(blockWithNoPage);
            await bindingContext.Pages.AddAsync(emptyPage);
            await bindingContext.Pages.AddAsync(pageWithBlocks);
            await bindingContext.Pages.AddAsync(pageWithChildrenChildren);

            await bindingContext.SaveChangesAsync();
        }
    }
}