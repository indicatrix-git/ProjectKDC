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
	
  [Implements(typeof(ICRUD<PravnickaOsoba>), ServiceName = "PravnickaOsoba")]
  [Implements(typeof(IBrowse<PravnickaOsoba>), ServiceName = "PravnickaOsoba")]
  [Implements(typeof(IPagedBrowse<PravnickaOsoba>), ServiceName = "PravnickaOsoba")]
    [Implements(typeof(PravnickaOsobaRepository))]
	public partial class PravnickaOsobaRepository : DataAccessBase, ICRUD<PravnickaOsoba>, IBrowse<PravnickaOsoba>, IPagedBrowse<PravnickaOsoba>
    {

		#region Create

		private const string PravnickaOsobaCreateQuery = "PravnickaOsobaCreate";

		public PravnickaOsoba Create(PravnickaOsoba dataObject)
		{
			try
			{
				PravnickaOsoba entry = (PravnickaOsoba)dataObject;
				// vytvori sa databazova session 
				using (ISession session = SessionProvider.OpenSession())
				{
					// nacitaju sa udaje z hbm suboru 
					IQuery query = session.GetNamedQuery(PravnickaOsobaCreateQuery);
					// vstupne parametre procedury sa naplnia udajmy z objektu 
					query.SetProperties(entry);
					query.SetParameter("TransakciaId", this.TransactionID);
					// zapis udajov do databazy 
                    return query.UniqueResult<PravnickaOsoba>();
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 101);
			}
		}

		#endregion Create

		#region Update

		private const string PravnickaOsobaUpdateQuery = "PravnickaOsobaUpdate";

		public PravnickaOsoba Update(PravnickaOsoba dataObject)
		{
			try
			{
				PravnickaOsoba entry = (PravnickaOsoba)dataObject;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(PravnickaOsobaUpdateQuery);
					query.SetProperties(entry);
					query.SetParameter("TransakciaId", this.TransactionID);
                    query.SetParameter("TRANSAKCIA_ID_OLD", dataObject.TransakciaId);
					query.ExecuteUpdate();

					dataObject.TransakciaId = this.TransactionID as Guid?;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 102);
			}
			return dataObject;
		}

		#endregion Update

		#region Delete

		private const string PravnickaOsobaDeleteQuery = "PravnickaOsobaDelete";

		public void Delete(PravnickaOsoba dataObject)
		{
			try
			{
				PravnickaOsoba entry = (PravnickaOsoba)dataObject;
				using (ISession session = SessionProvider.OpenSession())
				{
					// nacitaju sa udaje z hbm suboru 
					IQuery query = session.GetNamedQuery(PravnickaOsobaDeleteQuery);
					//entry.Stav = -1;
					// vstupne parametre procedury sa naplnia udajmi z objektu 
					query.SetProperties(entry);
                    query.SetParameter("TransakciaZruseneId", this.TransactionID);

					// zapis udajov do databazy 
					query.ExecuteUpdate();
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 103);
			}
		}

		#endregion Delete

		#region Get

		private const string PravnickaOsobaGetQuery = "PravnickaOsobaGet";

		public PravnickaOsoba Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(PravnickaOsobaGetQuery);
					query.SetProperties((PravnickaOsobaFilterCriteria)dataObject);
					var response = query.UniqueResult<PravnickaOsoba>();
					return (PravnickaOsoba)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string PravnickaOsobaListQuery = "PravnickaOsobaList";
        
		public IList<PravnickaOsoba> Browse(object criteria)
		{
			var response = new List<PravnickaOsoba>();
			try
			{
				var filterCriteria = criteria as PravnickaOsobaFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(PravnickaOsobaListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<PravnickaOsoba>().ToList();
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

		private const string PravnickaOsobaBrowseCntQuery = "PravnickaOsobaBrowseCnt";
        private const string PravnickaOsobaBrowsePgQuery = "PravnickaOsobaBrowsePg";
        
		public IList<PravnickaOsoba> PagedBrowse(ref RequestResult<List<PravnickaOsoba>> requestResult, object filterCriteria)
		{
			var response = new List<PravnickaOsoba>();
			var criteria = (PravnickaOsobaFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(PravnickaOsobaBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((PravnickaOsobaFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(PravnickaOsobaBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((PravnickaOsobaFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
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
                    response = query.List<PravnickaOsoba>().ToList();

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