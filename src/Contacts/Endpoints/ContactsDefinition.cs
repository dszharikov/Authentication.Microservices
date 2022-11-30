using AutoMapper;
using AutoMapper.QueryableExtensions;
using Calabonga.AspNetCore.AppDefinitions;
using Contacts.Domain;
using Contacts.Dto;
using Contacts.Dto.ContactLists;
using Contacts.Endpoints.ViewModel;
using Contacts.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Endpoints;

public class ContactsDefinition : AppDefinition
{
    public override void ConfigureApplication(WebApplication app)
    {
        app.MapGet("~/contacts", GetContactsListAsync).ExcludeFromDescription();
        app.MapGet("~/contact", GetDetailedInfoAsync).ExcludeFromDescription();
        app.MapPost("~/contact", UpdateContactAsync).ExcludeFromDescription();
        app.MapPut("~/contact", CreateContactAsync).ExcludeFromDescription();
        app.MapDelete("~/contact", DeleteContactAsync).ExcludeFromDescription();
    }

    private async Task<IResult> DeleteContactAsync(HttpContext context,
        [FromServices] ApplicationDbContext dbContext, [FromBody] DeleteContactViewModel viewModel)
    {
        var contact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.UserId == viewModel.UserId && c.Id == viewModel.Id);

        if (contact is null)
        {
            return Results.NotFound();
        }

        dbContext.Contacts.Remove(contact);

        var result = await dbContext.SaveChangesAsync();
        
        if (result == 0)
        {
            return Results.Ok();
        }

        return Results.BadRequest();
    }

    private async Task<IResult> CreateContactAsync(HttpContext context, [FromServices] IMapper mapper,
        [FromServices] ApplicationDbContext dbContext, [FromBody] ContactCreateViewModel viewModel)
    {
        var contact = mapper.Map<Contact>(viewModel);

        await dbContext.Contacts.AddAsync(contact);

        var result = await dbContext.SaveChangesAsync();
        
        if (result == 0)
        {
            return Results.Ok(contact);
        }

        return Results.BadRequest();
    }

    // todo: create dto to change contact class in this endpoint
    private async Task<IResult> UpdateContactAsync(HttpContext context,
        [FromServices] ApplicationDbContext dbContext, [FromBody] Contact contact)
    {
        var dbContact =
            await dbContext.Contacts.FirstOrDefaultAsync(c => c.UserId == contact.UserId && c.Id == contact.Id);

        if (dbContact is null)
        {
            return Results.NotFound();
        }

        dbContext.Contacts.Update(contact);

        var result = await dbContext.SaveChangesAsync();

        if (result == 0)
        {
            return Results.Ok();
        }

        return Results.BadRequest();
    }

    private async Task<IResult> GetDetailedInfoAsync(HttpContext context, [FromServices] IMapper mapper,
        [FromServices] ApplicationDbContext dbContext, [FromBody] DetailedContactViewModel viewModel)
    {
        var contact =
            await dbContext.Contacts.FirstOrDefaultAsync(
                contact => contact.UserId == viewModel.UserId && contact.Id == viewModel.Id);

        if (contact is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(mapper.Map<ContactDetailedDto>(contact));
    }

    private async Task<IResult> GetContactsListAsync(HttpContext context, [FromServices] IMapper mapper,
        [FromServices] ApplicationDbContext dbContext, [FromBody] ContactListViewModel viewModel)
    {
        var contacts =
            await dbContext.Contacts.Where(
                    contact => contact.UserId == viewModel.UserId)
                .ProjectTo<ContactLookupDto>(mapper.ConfigurationProvider).ToListAsync();

        if (contacts.Count > 0)
        {
            return Results.Ok(new ContactListDto { Contacts = contacts });
        }

        return Results.NotFound();
    }
}