using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.SysFra.DataAccess.Infrastructure;
using Ditec.SysFra.DataTypes.Infrastructure;
using Ditec.SysFra.Infrastructure.Impl;
using Ditec.SysFra.NhSql.Dal;
using LinFu.IoC.Configuration;
using NHibernate;

using Ditec.RIS.RFO.Dol;


namespace Ditec.RIS.RFO.Dal.Repository
{
	
  [Implements(typeof(ICRUD<StotoznenaFyzOsoba>), ServiceName = "StotoznenaFyzOsoba")]
  [Implements(typeof(ICRUDTransaction<StotoznenaFyzOsoba>), ServiceName = "StotoznenaFyzOsoba")]
  [Implements(typeof(IBrowse<StotoznenaFyzOsoba>), ServiceName = "StotoznenaFyzOsoba")]
  [Implements(typeof(IPagedBrowse<StotoznenaFyzOsoba>), ServiceName = "StotoznenaFyzOsoba")]
    [Implements(typeof(StotoznenaFyzOsobaRepository))]
	public partial class StotoznenaFyzOsobaRepository : DataAccessBase, ICRUD<StotoznenaFyzOsoba>, ICRUDTransaction<StotoznenaFyzOsoba>, IBrowse<StotoznenaFyzOsoba>, IPagedBrowse<StotoznenaFyzOsoba>
    {

		#region Create

		private const string StotoznenaFyzOsobaCreateQuery = "StotoznenaFyzOsobaCreate";

		public StotoznenaFyzOsoba Create(StotoznenaFyzOsoba dataObject)
		{
			try
			{
				// vytvori sa databazova session 
				using (ISession session = SessionProvider.OpenSession())
				{
					return Create(dataObject, session);
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 101);
			}
		}

		public StotoznenaFyzOsoba Create(StotoznenaFyzOsoba dataObject, ISession session)
		{
			try
			{
				StotoznenaFyzOsoba entry = (StotoznenaFyzOsoba)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(StotoznenaFyzOsobaCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<StotoznenaFyzOsoba>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create

		#region Update

		private const string StotoznenaFyzOsobaUpdateQuery = "StotoznenaFyzOsobaUpdate";

		public StotoznenaFyzOsoba Update(StotoznenaFyzOsoba dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					return Update(dataObject, session);
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 102);
			}
			return dataObject;
		}

		public StotoznenaFyzOsoba Update(StotoznenaFyzOsoba dataObject, ISession session)
		{
			try
			{
				StotoznenaFyzOsoba entry = (StotoznenaFyzOsoba)dataObject;
				IQuery query = session.GetNamedQuery(StotoznenaFyzOsobaUpdateQuery);
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
                query.SetParameter("TRANSAKCIA_ID_OLD", dataObject.TransakciaId);
				query.ExecuteUpdate();

				dataObject.TransakciaId = this.TransactionID as Guid?;
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 112);
			}
			return dataObject;
		}

		#endregion Update

		#region Delete

		private const string StotoznenaFyzOsobaDeleteQuery = "StotoznenaFyzOsobaDelete";

		public void Delete(StotoznenaFyzOsoba dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					Delete(dataObject, session);
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 103);
			}
		}

		public void Delete(StotoznenaFyzOsoba dataObject, ISession session)
		{
			try
			{
				StotoznenaFyzOsoba entry = (StotoznenaFyzOsoba)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(StotoznenaFyzOsobaDeleteQuery);
				//entry.Stav = -1;
				// vstupne parametre procedury sa naplnia udajmi z objektu 
				query.SetProperties(entry);
                query.SetParameter("TransakciaZruseneId", this.TransactionID);

				// zapis udajov do databazy 
				query.ExecuteUpdate();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 113);
			}
		}

		#endregion Delete

		#region Get

		private const string StotoznenaFyzOsobaGetQuery = "StotoznenaFyzOsobaGet";

		public StotoznenaFyzOsoba Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(StotoznenaFyzOsobaGetQuery);
					query.SetProperties((StotoznenaFyzOsobaFilterCriteria)dataObject);
					var response = query.UniqueResult<StotoznenaFyzOsoba>();
					return (StotoznenaFyzOsoba)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string StotoznenaFyzOsobaListQuery = "StotoznenaFyzOsobaList";
        
		public IList<StotoznenaFyzOsoba> Browse(object criteria)
		{
			var response = new List<StotoznenaFyzOsoba>();
			try
			{
				var filterCriteria = criteria as StotoznenaFyzOsobaFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(StotoznenaFyzOsobaListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<StotoznenaFyzOsoba>().ToList();
					return response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 105);
			}
		}

		#endregion Browse
		
		#region PagedBrowse

		private const string StotoznenaFyzOsobaBrowseCntQuery = "StotoznenaFyzOsobaBrowseCnt";
        private const string StotoznenaFyzOsobaBrowsePgQuery = "StotoznenaFyzOsobaBrowsePg";
        
		public IList<StotoznenaFyzOsoba> PagedBrowse(ref RequestResult<List<StotoznenaFyzOsoba>> requestResult, object filterCriteria)
		{
			var response = new List<StotoznenaFyzOsoba>();
			var criteria = (StotoznenaFyzOsobaFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(StotoznenaFyzOsobaBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((StotoznenaFyzOsobaFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(StotoznenaFyzOsobaBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((StotoznenaFyzOsobaFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
                        criteria.PagingInfo.CurrentPage = 1;

                    query.SetParameter("CurrentPage", criteria.PagingInfo.CurrentPage);
                    query.SetParameter("RecordsPerPage", criteria.PagingInfo.RecordsPerPage);
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}
                    response = query.List<StotoznenaFyzOsoba>().ToList();

                    // aktualizuje sa informacia o strankovanom zozname v obalke RequestResult 
                    requestResult.PagingInfo = criteria.PagingInfo;

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 106);
            }
		}

		#endregion PagedBrowse
	}
}