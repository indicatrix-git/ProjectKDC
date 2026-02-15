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
	
  [Implements(typeof(ICRUD<PravnaSposobilostObmedzenie>), ServiceName = "PravnaSposobilostObmedzenie")]
  [Implements(typeof(ICRUDTransaction<PravnaSposobilostObmedzenie>), ServiceName = "PravnaSposobilostObmedzenie")]
  [Implements(typeof(IBrowse<PravnaSposobilostObmedzenie>), ServiceName = "PravnaSposobilostObmedzenie")]
  [Implements(typeof(IPagedBrowse<PravnaSposobilostObmedzenie>), ServiceName = "PravnaSposobilostObmedzenie")]
    [Implements(typeof(PravnaSposobilostObmedzenieRepository))]
	public partial class PravnaSposobilostObmedzenieRepository : DataAccessBase, ICRUD<PravnaSposobilostObmedzenie>, ICRUDTransaction<PravnaSposobilostObmedzenie>, IBrowse<PravnaSposobilostObmedzenie>, IPagedBrowse<PravnaSposobilostObmedzenie>
    {

		#region Create

		private const string PravnaSposobilostObmedzenieCreateQuery = "PravnaSposobilostObmedzenieCreate";

		public PravnaSposobilostObmedzenie Create(PravnaSposobilostObmedzenie dataObject)
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

		public PravnaSposobilostObmedzenie Create(PravnaSposobilostObmedzenie dataObject, ISession session)
		{
			try
			{
				PravnaSposobilostObmedzenie entry = (PravnaSposobilostObmedzenie)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(PravnaSposobilostObmedzenieCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<PravnaSposobilostObmedzenie>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create

		#region Update

		private const string PravnaSposobilostObmedzenieUpdateQuery = "PravnaSposobilostObmedzenieUpdate";

		public PravnaSposobilostObmedzenie Update(PravnaSposobilostObmedzenie dataObject)
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

		public PravnaSposobilostObmedzenie Update(PravnaSposobilostObmedzenie dataObject, ISession session)
		{
			try
			{
				PravnaSposobilostObmedzenie entry = (PravnaSposobilostObmedzenie)dataObject;
				IQuery query = session.GetNamedQuery(PravnaSposobilostObmedzenieUpdateQuery);
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

		private const string PravnaSposobilostObmedzenieDeleteQuery = "PravnaSposobilostObmedzenieDelete";

		public void Delete(PravnaSposobilostObmedzenie dataObject)
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

		public void Delete(PravnaSposobilostObmedzenie dataObject, ISession session)
		{
			try
			{
				PravnaSposobilostObmedzenie entry = (PravnaSposobilostObmedzenie)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(PravnaSposobilostObmedzenieDeleteQuery);
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

		private const string PravnaSposobilostObmedzenieGetQuery = "PravnaSposobilostObmedzenieGet";

		public PravnaSposobilostObmedzenie Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(PravnaSposobilostObmedzenieGetQuery);
					query.SetProperties((PravnaSposobilostObmedzenieFilterCriteria)dataObject);
					var response = query.UniqueResult<PravnaSposobilostObmedzenie>();
					return (PravnaSposobilostObmedzenie)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string PravnaSposobilostObmedzenieListQuery = "PravnaSposobilostObmedzenieList";
        
		public IList<PravnaSposobilostObmedzenie> Browse(object criteria)
		{
			var response = new List<PravnaSposobilostObmedzenie>();
			try
			{
				var filterCriteria = criteria as PravnaSposobilostObmedzenieFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(PravnaSposobilostObmedzenieListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<PravnaSposobilostObmedzenie>().ToList();
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

		private const string PravnaSposobilostObmedzenieBrowseCntQuery = "PravnaSposobilostObmedzenieBrowseCnt";
        private const string PravnaSposobilostObmedzenieBrowsePgQuery = "PravnaSposobilostObmedzenieBrowsePg";
        
		public IList<PravnaSposobilostObmedzenie> PagedBrowse(ref RequestResult<List<PravnaSposobilostObmedzenie>> requestResult, object filterCriteria)
		{
			var response = new List<PravnaSposobilostObmedzenie>();
			var criteria = (PravnaSposobilostObmedzenieFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(PravnaSposobilostObmedzenieBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((PravnaSposobilostObmedzenieFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(PravnaSposobilostObmedzenieBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((PravnaSposobilostObmedzenieFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
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
                    response = query.List<PravnaSposobilostObmedzenie>().ToList();

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