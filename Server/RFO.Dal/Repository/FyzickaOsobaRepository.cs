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
	
  [Implements(typeof(ICRUD<FyzickaOsoba>), ServiceName = "FyzickaOsoba")]
  [Implements(typeof(ICRUDTransaction<FyzickaOsoba>), ServiceName = "FyzickaOsoba")]
  [Implements(typeof(IBrowse<FyzickaOsoba>), ServiceName = "FyzickaOsoba")]
  [Implements(typeof(IPagedBrowse<FyzickaOsoba>), ServiceName = "FyzickaOsoba")]
    [Implements(typeof(FyzickaOsobaRepository))]
	public partial class FyzickaOsobaRepository : DataAccessBase, ICRUD<FyzickaOsoba>, ICRUDTransaction<FyzickaOsoba>, IBrowse<FyzickaOsoba>, IPagedBrowse<FyzickaOsoba>
    {

		#region Create

		private const string FyzickaOsobaCreateQuery = "FyzickaOsobaCreate";

		public FyzickaOsoba Create(FyzickaOsoba dataObject)
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

		public FyzickaOsoba Create(FyzickaOsoba dataObject, ISession session)
		{
			try
			{
				FyzickaOsoba entry = (FyzickaOsoba)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(FyzickaOsobaCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<FyzickaOsoba>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create		

        #region Update

		private const string FyzickaOsobaUpdateQuery = "FyzickaOsobaUpdate";

		public FyzickaOsoba Update(FyzickaOsoba dataObject)
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

		public FyzickaOsoba Update(FyzickaOsoba dataObject, ISession session)
		{
			try
			{
				FyzickaOsoba entry = (FyzickaOsoba)dataObject;
				IQuery query = session.GetNamedQuery(FyzickaOsobaUpdateQuery);
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

		private const string FyzickaOsobaDeleteQuery = "FyzickaOsobaDelete";

		public void Delete(FyzickaOsoba dataObject)
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

		public void Delete(FyzickaOsoba dataObject, ISession session)
		{
			try
			{
				FyzickaOsoba entry = (FyzickaOsoba)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(FyzickaOsobaDeleteQuery);
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

		private const string FyzickaOsobaGetQuery = "FyzickaOsobaGet";

		public FyzickaOsoba Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(FyzickaOsobaGetQuery);
					query.SetProperties((FyzickaOsobaFilterCriteria)dataObject);
					var response = query.UniqueResult<FyzickaOsoba>();
					return (FyzickaOsoba)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string FyzickaOsobaListQuery = "FyzickaOsobaList";
        
		public IList<FyzickaOsoba> Browse(object criteria)
		{
			var response = new List<FyzickaOsoba>();
			try
			{
				var filterCriteria = criteria as FyzickaOsobaFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(FyzickaOsobaListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<FyzickaOsoba>().ToList();
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

		private const string FyzickaOsobaBrowseCntQuery = "FyzickaOsobaBrowseCnt";
        private const string FyzickaOsobaBrowsePgQuery = "FyzickaOsobaBrowsePg";
        
		public IList<FyzickaOsoba> PagedBrowse(ref RequestResult<List<FyzickaOsoba>> requestResult, object filterCriteria)
		{
			var response = new List<FyzickaOsoba>();
			var criteria = (FyzickaOsobaFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(FyzickaOsobaBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((FyzickaOsobaFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(FyzickaOsobaBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((FyzickaOsobaFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
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
                    response = query.List<FyzickaOsoba>().ToList();

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

		#region FyzickaOsobaEduIdGet

		private const string FyzickaOsobaEduIdGetQuery = "FyzickaOsobaEduIdGet";

		public FyzickaOsoba FyzickaOsobaEduIdGet(string EduId)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(FyzickaOsobaEduIdGetQuery);
					query.SetParameter("EDUID", EduId);
					var response = query.UniqueResult<FyzickaOsoba>();
					return (FyzickaOsoba)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 107);
			}
		}

		#endregion FyzickaOsobaEduIdGet

		#region FyzickaOsobaEduIdStotoznenaGet

		private const string FyzickaOsobaEduIdStotoznenaGetQuery = "FyzickaOsobaEduIdStotoznenaGet";

		public FyzickaOsoba FyzickaOsobaEduIdStotoznenaGet(string EduId)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(FyzickaOsobaEduIdStotoznenaGetQuery);
					query.SetParameter("EDUID", EduId);
					var response = query.UniqueResult<FyzickaOsoba>();
					return (FyzickaOsoba)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 107);
			}
		}

		#endregion FyzickaOsobaEduIdStotoznenaGet
	}
}