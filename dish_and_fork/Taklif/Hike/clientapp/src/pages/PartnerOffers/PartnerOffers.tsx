import { useSnackbar } from 'notistack';
import { FC, useEffect, useState } from 'react';

import { useGetSellerIndividualApplicationOffersMyFilter } from '~/api';
import { useDeleteIndividualOrderOffer } from '~/api/v1/useDeleteIndividualOrderOffer';
import { EmptyList, Error, LoadingSpinner, PageLayout, Pagination } from '~/components';
import ConfirmDeleteDialog from '~/components/ConfirmDeleteDialog/ConfirmDeleteDialog';
import { useConfig } from '~/contexts';
import { useDownSm, usePageNumber } from '~/hooks';
import { getPartnerNewAppsPath } from '~/routing';

import { PartnerOffersList } from './PartnerOffersList/PartnerOffersList';
import { PartnerOffersTable } from './PartnerOffersTable/PartnerOffersTable';

const PartnerOffers: FC = () => {
  const isXs = useDownSm();
  const [pageNumber] = usePageNumber();
  const { applications } = useConfig();
  const { enqueueSnackbar } = useSnackbar();
  const { pageSize } = applications;
  const { data, isError, isLoading, refetch } = useGetSellerIndividualApplicationOffersMyFilter(pageSize, pageNumber);
  const [deleteOfferId, setDeleteOfferId] = useState<undefined | string>(undefined);
  const deleteOffer = useDeleteIndividualOrderOffer();

  const pagesCount = data && Math.ceil(data.totalCount / pageSize);

  const onDelete = (offerId: string) => {
    setDeleteOfferId(offerId);
  };

  const onDeleteClose = () => {
    setDeleteOfferId(undefined);
  };

  const onDeleteConfirmed = () => {
    if (deleteOfferId) {
      deleteOffer.mutate({
        offerId: deleteOfferId,
      });
    }
  };

  useEffect(() => {
    if (deleteOffer.isSuccess) {
      enqueueSnackbar('Ваше предложение удалено', {
        variant: 'success',
      });
      refetch();
      onDeleteClose();
    }
  }, [deleteOffer.isSuccess, enqueueSnackbar, refetch]);

  useEffect(() => {
    if (deleteOffer.isError) {
      enqueueSnackbar(String(deleteOffer.error), {
        variant: 'error',
      });

      onDeleteClose();
    }
  }, [deleteOffer.error, deleteOffer.isError, enqueueSnackbar, refetch]);

  if (isError) {
    return <Error />;
  }

  if (!data && isLoading) {
    return <LoadingSpinner />;
  }

  return (
    <PageLayout title="Мои отклики">
      <>
        {!data?.totalCount || !data.items?.length ? (
          <EmptyList buttonText="Посмотреть заявки" to={getPartnerNewAppsPath()}>
            Окликов нет
          </EmptyList>
        ) : isXs ? (
          <>
            <PartnerOffersList isLoading={isLoading} rows={data.items} onDelete={onDelete} />
          </>
        ) : (
          <>
            <PartnerOffersTable rows={data.items} onDelete={onDelete} />
          </>
        )}
      </>

      {data?.totalCount ? <Pagination pageNumber={pageNumber} pagesCount={pagesCount} /> : undefined}

      <ConfirmDeleteDialog
        text="Вы уверены что хотите удалить Вашу заявку на индивидуальный заказ?"
        itemId={deleteOfferId}
        onClose={onDeleteClose}
        onDelete={onDeleteConfirmed}
      />
    </PageLayout>
  );
};

export { PartnerOffers };
