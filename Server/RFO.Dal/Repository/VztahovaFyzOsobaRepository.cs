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
	
  [Implements(typeof(ICRUD<VztahovaFyzOsoba>), ServiceName = "VztahovaFyzOsoba")]
  [Implements(typeof(ICRUDTransaction<VztahovaFyzOsoba>), ServiceName = "VztahovaFyzOsoba")]
  [Implements(typeof(IBrowse<VztahovaFyzOsoba>), ServiceName = "VztahovaFyzOsoba")]
  [Implements(typeof(IPagedBrowse<VztahovaFyzOsoba>), ServiceName = "VztahovaFyzOsoba")]
    [Implements(typeof(VztahovaFyzOsobaRepository))]
	public partial class VztahovaFyzOsobaRepository : DataAccessBase, ICRUD<VztahovaFyzOsoba>, ICRUDTransaction<VztahovaFyzOsoba>, IBrowse<VztahovaFyzOsoba>, IPagedBrowse<VztahovaFyzOsoba>
    {

		#region Create

		private const string VztahovaFyzOsobaCreateQuery = "VztahovaFyzOsobaCreate";

		public VztahovaFyzOsoba Create(VztahovaFyzOsoba dataObject)
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

		public VztahovaFyzOsoba Create(VztahovaFyzOsoba dataObject, ISession session)
		{
			try
			{
				VztahovaFyzOsoba entry = (VztahovaFyzOsoba)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(VztahovaFyzOsobaCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<VztahovaFyzOsoba>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create

		#region Update

		private const string VztahovaFyzOsobaUpdateQuery = "VztahovaFyzOsobaUpdate";

		public VztahovaFyzOsoba Update(VztahovaFyzOsoba dataObject)
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

		public VztahovaFyzOsoba Update(VztahovaFyzOsoba dataObject, ISession session)
		{
			try
			{
				VztahovaFyzOsoba entry = (VztahovaFyzOsoba)dataObject;
				IQuery query = session.GetNamedQuery(VztahovaFyzOsobaUpdateQuery);
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

		private const string VztahovaFyzOsobaDeleteQuery = "VztahovaFyzOsobaDelete";

		public void Delete(VztahovaFyzOsoba dataObject)
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

		public void Delete(VztahovaFyzOsoba dataObject, ISession session)
		{
			try
			{
				VztahovaFyzOsoba entry = (VztahovaFyzOsoba)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(VztahovaFyzOsobaDeleteQuery);
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

		private const string VztahovaFyzOsobaGetQuery = "VztahovaFyzOsobaGet";

		public VztahovaFyzOsoba Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(VztahovaFyzOsobaGetQuery);
					query.SetProperties((VztahovaFyzOsobaFilterCriteria)dataObject);
					var response = query.UniqueResult<VztahovaFyzOsoba>();
					return (VztahovaFyzOsoba)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string VztahovaFyzOsobaListQuery = "VztahovaFyzOsobaList";
        
		public IList<VztahovaFyzOsoba> Browse(object criteria)
		{
			var response = new List<VztahovaFyzOsoba>();
			try
			{
				var filterCriteria = criteria as VztahovaFyzOsobaFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(VztahovaFyzOsobaListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<VztahovaFyzOsoba>().ToList();
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

		private const string VztahovaFyzOsobaBrowseCntQuery = "VztahovaFyzOsobaBrowseCnt";
        private const string VztahovaFyzOsobaBrowsePgQuery = "VztahovaFyzOsobaBrowsePg";
        
		public IList<VztahovaFyzOsoba> PagedBrowse(ref RequestResult<List<VztahovaFyzOsoba>> requestResult, object filterCriteria)
		{
			var response = new List<VztahovaFyzOsoba>();
			var criteria = (VztahovaFyzOsobaFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(VztahovaFyzOsobaBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((VztahovaFyzOsobaFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(VztahovaFyzOsobaBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((VztahovaFyzOsobaFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
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
                    response = query.List<VztahovaFyzOsoba>().ToList();

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