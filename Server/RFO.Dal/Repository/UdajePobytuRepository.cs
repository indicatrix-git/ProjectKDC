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
	
  [Implements(typeof(ICRUD<UdajePobytu>), ServiceName = "UdajePobytu")]
  [Implements(typeof(ICRUDTransaction<UdajePobytu>), ServiceName = "UdajePobytu")]
  [Implements(typeof(IBrowse<UdajePobytu>), ServiceName = "UdajePobytu")]
  [Implements(typeof(IPagedBrowse<UdajePobytu>), ServiceName = "UdajePobytu")]
    [Implements(typeof(UdajePobytuRepository))]
	public partial class UdajePobytuRepository : DataAccessBase, ICRUD<UdajePobytu>, ICRUDTransaction<UdajePobytu>, IBrowse<UdajePobytu>, IPagedBrowse<UdajePobytu>
    {

		#region Create

		private const string UdajePobytuCreateQuery = "UdajePobytuFOCreate";

		public UdajePobytu Create(UdajePobytu dataObject)
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

		public UdajePobytu Create(UdajePobytu dataObject, ISession session)
		{
			try
			{
				UdajePobytu entry = (UdajePobytu)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(UdajePobytuCreateQuery);
				// vstupne parametre procedury sa naplnia udajmy z objektu 
				query.SetProperties(entry);
				query.SetParameter("TransakciaId", this.TransactionID);
				// zapis udajov do databazy 
                return query.UniqueResult<UdajePobytu>();
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
			}
		}

		#endregion Create

		#region Update

		private const string UdajePobytuUpdateQuery = "UdajePobytuFOUpdate";

		public UdajePobytu Update(UdajePobytu dataObject)
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

		public UdajePobytu Update(UdajePobytu dataObject, ISession session)
		{
			try
			{
				UdajePobytu entry = (UdajePobytu)dataObject;
				IQuery query = session.GetNamedQuery(UdajePobytuUpdateQuery);
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

		private const string UdajePobytuDeleteQuery = "UdajePobytuFODelete";

		public void Delete(UdajePobytu dataObject)
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

		public void Delete(UdajePobytu dataObject, ISession session)
		{
			try
			{
				UdajePobytu entry = (UdajePobytu)dataObject;
				// nacitaju sa udaje z hbm suboru 
				IQuery query = session.GetNamedQuery(UdajePobytuDeleteQuery);
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

		private const string UdajePobytuGetQuery = "UdajePobytuFOGet";

		public UdajePobytu Read(object dataObject)
		{
			try
			{
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(UdajePobytuGetQuery);
					query.SetProperties((UdajePobytuFilterCriteria)dataObject);
					var response = query.UniqueResult<UdajePobytu>();
					return (UdajePobytu)response;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
			}
		}

		#endregion Get

		#region Browse

		private const string UdajePobytuListQuery = "UdajePobytuFOList";
        
		public IList<UdajePobytu> Browse(object criteria)
		{
			var response = new List<UdajePobytu>();
			try
			{
				var filterCriteria = criteria as UdajePobytuFilterCriteria;
				using (ISession session = SessionProvider.OpenSession())
				{
					IQuery query = session.GetNamedQuery(UdajePobytuListQuery);
					query.SetProperties(filterCriteria);
					
					if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if(filterCriteria.DatumPlatnosti.HasValue)
							query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
						else
							query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

					response = query.List<UdajePobytu>().ToList();
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

		private const string UdajePobytuBrowseCntQuery = "UdajePobytuFOBrowseCnt";
        private const string UdajePobytuBrowsePgQuery = "UdajePobytuFOBrowsePg";
        
		public IList<UdajePobytu> PagedBrowse(ref RequestResult<List<UdajePobytu>> requestResult, object filterCriteria)
		{
			var response = new List<UdajePobytu>();
			var criteria = (UdajePobytuFilterCriteria)filterCriteria;

            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery queryCnt = session.GetNamedQuery(UdajePobytuBrowseCntQuery);
                    queryCnt.SetProperties(criteria);
					if (queryCnt.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
						if (criteria.DatumPlatnosti.HasValue)
							queryCnt.SetParameter("DATUM_SIMULACIE", criteria.DatumPlatnosti.Value);
						else
							queryCnt.SetParameter("DATUM_SIMULACIE", DateTime.Now);
					}

                    ((UdajePobytuFilterCriteria)filterCriteria).PagingInfo.TotalRecords = queryCnt.UniqueResult<int>();

                    IQuery query = session.GetNamedQuery(UdajePobytuBrowsePgQuery);
                    query.SetProperties(criteria);

					 if (((UdajePobytuFilterCriteria)filterCriteria).PagingInfo.TotalRecords > 0 && criteria.PagingInfo.CurrentPage == 0)
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
                    response = query.List<UdajePobytu>().ToList();

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