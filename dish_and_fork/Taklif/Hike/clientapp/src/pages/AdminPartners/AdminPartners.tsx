import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { FC, useEffect } from 'react';

import { useGetAdminPartners } from '~/api/v1/useGetAdminPartners';
import { EmptyList, Error, LoadingSpinner, PageLayout } from '~/components';
import { useDownSm } from '~/hooks';

import { RenderActionsCell, UPDATED_ADMIN_PARTNERS_TABLE } from './RenderActionsCell';
import { RenderExternalIdCell } from './RenderActionsCell/RenderExternalIdCell';
import { RenderPartnerStateCell } from './RenderPartnerStateCell';
import { typeValueGetter } from './typeValueGetter';

const columns: GridColDef[] = [
  {
    field: 'actions',
    headerName: 'Действия',
    renderCell: RenderActionsCell,
    renderHeader: () => null,
    sortable: false,
    width: 305,
  },

  {
    field: 'title',
    flex: 1,
    headerName: 'Наименование или ФИО',
  },

  {
    field: 'inn',
    flex: 1,
    headerName: 'ИНН',
  },

  {
    field: 'contactEmail',
    flex: 1,
    headerName: 'Электронная почта',
  },

  {
    field: 'contactPhone',
    headerName: 'Номер телефона',
    width: 150,
  },

  {
    field: 'type',
    headerName: 'Тип',
    valueGetter: typeValueGetter,
    width: 130,
  },

  {
    field: 'state',
    headerName: 'Статус',
    renderCell: RenderPartnerStateCell,
    width: 160,
  },
  {
    field: 'externalId',
    headerName: 'Id из YKassa',
    width: 180,
    renderCell: RenderExternalIdCell,
  },
];

const AdminPartners: FC = () => {
  const { data, isError, isLoading, refetch } = useGetAdminPartners();
  const isXs = useDownSm();

  useEffect(() => {
    const receiveMessage = (e: any) => {
      if (e.data === UPDATED_ADMIN_PARTNERS_TABLE) {
        refetch();
      }
    };

    window.addEventListener('message', receiveMessage, false);

    return () => {
      window.removeEventListener('message', receiveMessage);
    };
  }, []);

  if (isError) {
    return <Error />;
  }

  if (isLoading) {
    return <LoadingSpinner />;
  }

  const noData = !data || data.length < 0;

  return (
    <PageLayout title={isXs ? 'Продавцы' : 'Управление продавцами'}>
      {noData ? (
        <EmptyList>Список продавцов пуст</EmptyList>
      ) : (
        <DataGrid
          columns={columns}
          disableColumnFilter
          disableColumnMenu
          disableColumnSelector
          disableDensitySelector
          hideFooter
          pageSize={100}
          rows={data}
          sx={{ backgroundColor: 'background.paper', mb: 3 }}
        />
      )}
    </PageLayout>
  );
};

export { AdminPartners };
