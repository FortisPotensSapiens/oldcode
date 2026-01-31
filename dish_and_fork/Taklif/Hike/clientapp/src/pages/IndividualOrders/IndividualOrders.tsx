import styled from '@emotion/styled';
import AddIcon from '@mui/icons-material/Add';
import { Box, Button } from '@mui/material';
import { useSnackbar } from 'notistack';
import { useCallback, useEffect, useState } from 'react';

import { useGetIndividualOrdersByFilter } from '~/api';
import { useDeleteIndividualOrder } from '~/api/v1/useDeleteIndividualOrder';
import { LoadingSpinner } from '~/components';
import ConfirmDeleteDialog from '~/components/ConfirmDeleteDialog/ConfirmDeleteDialog';
import FabButton from '~/components/FabButton/FabButton';
import { PageLayout } from '~/components/PageLayout';

import { CreateIndividualOrderDialog } from './CreateIndividualOrderDialog';
import { IndividualOrdersTable, ROW_COUNT_ITEMS } from './IndividualOrdersTable';
import { NoOrders } from './NoOrders';

const TitleContainer = styled.div`
  display: flex;
  justify-content: center;
  white-space: pre;
`;

const IndividualOrders = () => {
  const [open, setOpen] = useState(false);
  const [page, setPage] = useState(0);
  const { enqueueSnackbar } = useSnackbar();
  const [rowsPerPage, setRowsPerPage] = useState(ROW_COUNT_ITEMS[0]);
  const { data, isFetching, isSuccess, refetch } = useGetIndividualOrdersByFilter({
    pageNumber: page + 1,
    pageSize: rowsPerPage,
  });
  const [deleteOrderId, setDeleteOrderId] = useState<undefined | string>(undefined);
  const deleteOrder = useDeleteIndividualOrder();

  const handleChangePage = (_event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const openDialog = () => {
    setOpen(true);
  };

  const closeDialog = useCallback(() => {
    setOpen(false);
    refetch();
  }, [refetch]);

  const onDelete = (offerId: string) => {
    setDeleteOrderId(offerId);
  };

  const onDeleteClose = () => {
    setDeleteOrderId(undefined);
  };

  const onDeleteConfirmed = () => {
    if (deleteOrderId) {
      deleteOrder.mutate({
        orderId: deleteOrderId,
      });
    }
  };

  useEffect(() => {
    if (deleteOrder.isSuccess) {
      enqueueSnackbar('Ваш индивидуальный заказ удален', {
        variant: 'success',
      });
      refetch();
      onDeleteClose();
    }
  }, [deleteOrder.isSuccess, enqueueSnackbar, refetch]);

  useEffect(() => {
    if (deleteOrder.isError) {
      enqueueSnackbar(String(deleteOrder.error), {
        variant: 'error',
      });

      onDeleteClose();
    }
  }, [deleteOrder.error, deleteOrder.isError, enqueueSnackbar, refetch]);

  return (
    <PageLayout
      title={
        <TitleContainer>
          Индивидуальные заказы
          {data?.totalCount ? (
            <Box
              borderRadius={50}
              component={Button}
              display={{ sm: 'inline-flex', xs: 'none' }}
              height={50}
              ml={3}
              onClick={openDialog}
              size="large"
              startIcon={<AddIcon />}
              variant="contained"
            >
              Создать заказ
            </Box>
          ) : undefined}
        </TitleContainer>
      }
    >
      <CreateIndividualOrderDialog handleClose={closeDialog} isOpen={open} />

      <FabButton onClick={openDialog} />

      {isFetching ? (
        <LoadingSpinner />
      ) : (
        <>
          {data?.totalCount === 0 ? <NoOrders handleOpenDialog={openDialog} /> : undefined}

          {isSuccess ? (
            <IndividualOrdersTable
              data={data}
              handleChangePage={handleChangePage}
              handleChangeRowsPerPage={handleChangeRowsPerPage}
              page={page}
              rowsPerPage={rowsPerPage}
              onDelete={onDelete}
            />
          ) : undefined}
        </>
      )}

      <ConfirmDeleteDialog
        text="Вы уверены что хотите удалить индивидуальный заказ?"
        itemId={deleteOrderId}
        onClose={onDeleteClose}
        onDelete={onDeleteConfirmed}
      />
    </PageLayout>
  );
};

export default IndividualOrders;
