﻿using FactsThrowingAPI.DAL.DAO;
using FactsThrowingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FactsThrowingAPI.DAL
{
    public class FactRepository : IRepository<Fact>
    {

        private readonly FactsContext _Context;
        public FactRepository(FactsContext context)
        {
            _Context = context;

        }


        public Fact Add(Fact entity)
        {
            _Context.Facts.Add(
                new FactDAO()
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Content = entity.Content
                });

            _Context.SaveChanges();

            return entity;
        }

        public async Task<Fact?> FindAsync(Guid Id)
        {
            var fact = await _Context.Facts.Where(c => c.Id == Id).FirstOrDefaultAsync();

            if (fact is null) { return null; }



            return new Fact(fact.Id, fact.Title, fact.Content);

        }

        public IList<Fact> List()
        {
            IList<Fact> result = new List<Fact>();

            var factsDAO = _Context.Facts.ToList();

            foreach (var fact in factsDAO)
            {

                result.Add(new Fact(fact.Id, fact.Title, fact.Content));
            }

            return result;

        }

        public Fact? Remove(Guid id)
        {
            var entity = _Context.Facts.Single(c => c.Id == id
                );

            if(entity is null)
            {
                return null;
            }

            var tracked = _Context.Facts.Remove(entity
               );

            var deletedEntity = tracked.Entity;
            _Context.SaveChanges();

            
            return new Fact(deletedEntity.Id, deletedEntity.Title, deletedEntity.Content) ;
        }

        public Fact? Update(Guid Id, Fact entity)
        {
            var updatedEntity = _Context.Facts.SingleOrDefault(c => c.Id == Id);

            if (updatedEntity != null)
            {
                updatedEntity.Title = entity.Title;
                updatedEntity.Content = entity.Content;
                _Context.Facts.Update(updatedEntity);
                _Context.SaveChanges();
                return new Fact(updatedEntity.Id, updatedEntity.Title, updatedEntity.Content);
            }

            else return null;
        }


        public List<Tag> RelatedTags(Guid factId)
        {
            var taglist = _Context.Facts_Tags.Where(c => c.IdFact == factId).Select(c => c.Tag).AsEnumerable();
            var result = new List<Tag>();
            foreach (var tag in taglist)
            {
                result.Add(new Tag(tag.Id, tag.Name));
            }

            return result;
        }

    }
}
