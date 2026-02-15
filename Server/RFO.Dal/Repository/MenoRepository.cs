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
	
  [Implements(typeof(ICRUD<Meno>), ServiceName = "Meno")]
  [Implements(typeof(ICRUDTransaction<Meno>), ServiceName = "Meno")]
  [Implements(typeof(IBrowse<Meno>), ServiceName = "Meno")]
  [Implements(typeof(IPagedBrowse<Meno>), ServiceName = "Meno")]
    [Implements(typeof(MenoRepository))]
	public partial class MenoRepository : DataAccessBase, ICRUD<Meno>, ICRUDTransaction<Meno>, IBrowse<Meno>, IPagedBrowse<Meno>
    {

		#region Create

		private const string MenoCreateQuery = "MenoCreate";

		public Meno Create(Meno dataObject)
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

		public Meno Create(Meno dataObject, ISession session)
		{
			try
			{
				Meno entry = (Meno)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(MenoCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<Meno>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create
		#region Update

		private const string MenoUpdateQuery = "MenoUpdate";

		public Meno Update(Meno dataObject)
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

		public Meno Update(Meno dataObject, ISession session)
		{
			try
			{
				Meno entry = (Meno)dataObject;
				IQuery query = session.GetNamedQuery(MenoUpdateQuery);
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

		private const string MenoDeleteQuery = "MenoDelete";

		public void Delete(Meno dataObject)
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

		public void Delete(Meno dataObject, ISession session)
		{
			try
			{
				Meno entry = (Meno)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(MenoDeleteQuery);
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

		private const string MenoGetQuery = "MenoGet";

		public Meno Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(MenoGetQuery);
					query.SetProperties((MenoFilterCriteria)dataObject);
					var response = query.UniqueResult<Meno>();
					return (Meno)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string MenoListQuery = "MenoList";
        
		public IList<Meno> Browse(object criteria)
		{
			var response = new List<Meno>();
			try
			{
				var filterCriteria = criteria as MenoFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(MenoListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<Meno>().ToList();
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

		private const string MenoBrowseCntQuery = "MenoBrowseCnt";
        private const string MenoBrowsePgQuery = "MenoBrowsePg";
        
		public IList<Meno> PagedBrowse(ref RequestResult<List<Meno>> requestResult, object filterCriteria)
		{
			var response = new List<Meno>();
			var criteria = (MenoFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(MenoBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((MenoFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(MenoBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((MenoFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
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
                    response = query.List<Meno>().ToList();

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