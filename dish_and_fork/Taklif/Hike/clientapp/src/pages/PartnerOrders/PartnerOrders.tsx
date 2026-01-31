import { Box, Breadcrumbs, Paper, Table } from '@mui/material';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { useGetSellerOrdersByFilter } from '~/api';
import { usePostDeliveryOrder } from '~/api/v1/usePostDeliveryOrder';
import { Link, LoadingSpinner, PageLayout, TablePagination } from '~/components';
import { useDownSm } from '~/hooks';
import { generatePartnerOrderInfoPath, getPartnerProductsPath } from '~/routing';

import { PartnerOrdersList } from './PartnerOrdersList';
import { PartnerOrdersTable } from './PartnerOrdersTable';
import { getRows, Row } from './utils';

const ROW_COUNT_ITEMS = [5, 10, 25, 50];

const PartnerOrders = () => {
  const navigate = useNavigate();
  const isXs = useDownSm();
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(ROW_COUNT_ITEMS[0]);
  const { data, isFetching, refetch } = useGetSellerOrdersByFilter({
    pageNumber: page + 1,
    pageSize: rowsPerPage,
  });
  const [rows, setRows] = useState<Row[]>([]);
  const placeDeliveryMutation = usePostDeliveryOrder();

  useEffect(() => {
    setRows(getRows(data) ?? []);
  }, [data]);

  const getPagination = () => {
    return (
      <TablePagination
        colSpan={5}
        count={data?.totalCount ?? 0}
        onPageChange={handleChangePage}
        onRowsPerPageChange={handleChangeRowsPerPage}
        page={page}
        rowsPerPage={rowsPerPage}
        rowsPerPageOptions={ROW_COUNT_ITEMS}
      />
    );
  };

  const handleChangePage = (_event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const onRowClick = (orderId: string) => {
    navigate(generatePartnerOrderInfoPath(orderId));
  };

  const onReadyToDeliverClick = (orderId: string) => {
    placeDeliveryMutation.mutate({ orderId });
  };

  useEffect(() => {
    if (placeDeliveryMutation.isSuccess) {
      refetch();
    }
  }, [placeDeliveryMutation.isSuccess, refetch]);

  return (
    <PageLayout
      beforeHeader={
        <Box display={{ md: 'block', xs: 'none' }}>
          <Breadcrumbs>
            <Link key="root" color="text.secondary" sx={{ textDecoration: 'none' }} to={getPartnerProductsPath()}>
              Все изделия
            </Link>

            <Box key="title" color="text.primary">
              Продажи
            </Box>
          </Breadcrumbs>
        </Box>
      }
      noHeaderTopPadding
      paddingTop={2}
      title="Продажи"
    >
      {isFetching && <LoadingSpinner />}

      {data?.totalCount === 0 ? <Paper>Нет продаж</Paper> : undefined}

      {rows?.length ? (
        isXs ? (
          <PartnerOrdersList
            footer={<Table>{getPagination()}</Table>}
            onRowClick={onRowClick}
            rows={rows}
            onReadyToDeliverClick={onReadyToDeliverClick}
          />
        ) : (
          <PartnerOrdersTable
            footer={getPagination()}
            onReadyToDeliverClick={onReadyToDeliverClick}
            onRowClick={onRowClick}
            rows={rows}
          />
        )
      ) : undefined}
    </PageLayout>
  );
};

export { PartnerOrders };
