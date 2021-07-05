using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DataAccess.EF.Extensions
{
    public static class RemoveForeignKeyExetension
    {
        public static ModelBuilder RemoveForeignKeys(this ModelBuilder modelBuilder)

        {

            var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();

            for (int i = 0; i < entityTypes.Count; i++)

            {

                var entityType = entityTypes[i];

                var references = entityType.GetDeclaredReferencingForeignKeys().ToList();

                using (((Model)entityType.Model).Builder.Metadata.ConventionDispatcher.DelayConventions())

                {

                    foreach (var reference in references)

                    {

                        reference.DeclaringEntityType.RemoveForeignKey(reference);

                    }

                }

            }

            return modelBuilder;

        }
    }
}

