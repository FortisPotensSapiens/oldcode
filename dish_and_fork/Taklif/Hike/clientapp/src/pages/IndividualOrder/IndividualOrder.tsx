import Grid from '@mui/material/Grid';
import { useSnackbar } from 'notistack';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

import { useGetMyUserProfile, useGetPartnerMy } from '~/api';
import { useGetIndividualOrder } from '~/api/v1/useGetIndividualOrder';
import { LoadingSpinner, PageLayout } from '~/components';
import { useConfig } from '~/contexts';

import { IndividualOrdersTableItem } from '../IndividualOrders/IndividualOrdersTable/IndividualOrdersTableItem';
import { useRedirect } from '../PartnerNewApps/useRedirect';
import { IndividualOrderResponses } from './IndividualOrderResponses';
import { IndividualOrderChatDialog } from './IndividualOrderResponses/IndividualOrderChatDialog';

const IndividualOrder = () => {
  const { enqueueSnackbar } = useSnackbar();
  const { individualOrderId = '' } = useParams();
  const [selectedOfferId, setSelectedOfferId] = useState<string | null>(null);
  const { data: myData } = useGetMyUserProfile();
  const { data: partnerMyData } = useGetPartnerMy();
  const { roles } = useConfig();
  const redirect = useRedirect();

  const { data, isError, isLoading, isSuccess } = useGetIndividualOrder(individualOrderId);

  useEffect(() => {
    if (isError) {
      enqueueSnackbar('Ошибка загрузки данных', {
        variant: 'error',
      });
    }
  }, [isError, enqueueSnackbar]);

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (isError) {
    return <PageLayout title="Ошибка загрузки данных" />;
  }

  const onChatOpenCallback = (offerId: string) => {
    setSelectedOfferId(offerId);
  };

  const onChatHide = () => {
    setSelectedOfferId(null);
  };

  const onCompleteCallback = (offerId: string) => {
    redirect(individualOrderId, offerId);
  };

  return isSuccess ? (
    <PageLayout title={data.title}>
      <Grid
        container
        flexDirection={{
          sm: 'column',
          xs: 'column-reverse',
        }}
        flexWrap={{
          sm: 'wrap',
          xs: 'revert',
        }}
        spacing={4}
      >
        <Grid item paddingTop={{ sm: 0, xs: 2 }} sm={7} xs={12}>
          {myData ? (
            <IndividualOrderResponses
              individualOrderId={individualOrderId}
              offers={data.offers ?? []}
              individualOrder={data}
              onChatOpen={onChatOpenCallback}
              my={myData}
              partnerMy={partnerMyData}
              showSelectSeller={!myData?.roles?.includes(roles.seller)}
            />
          ) : undefined}
        </Grid>
        <Grid item sm={5} xs={12} marginTop={1}>
          <IndividualOrdersTableItem
            row={data}
            showDescription
            showLogin={myData?.roles?.includes(roles.seller)}
            showOfferButton={myData?.roles?.includes(roles.seller)}
            onComplete={onCompleteCallback}
          />
        </Grid>
        {selectedOfferId ? (
          <IndividualOrderChatDialog onHide={onChatHide} title={String(data.title)} offerId={selectedOfferId} />
        ) : undefined}
      </Grid>
    </PageLayout>
  ) : (
    <></>
  );
};

export { IndividualOrder };
