import { Box, Grid, Paper, styled, Typography } from '@mui/material';
import dayjs from 'dayjs';
import LocalizedFormat from 'dayjs/plugin/localizedFormat';
import { useParams } from 'react-router-dom';

import { useGetSellerOrderById } from '~/api';
import { LoadingSpinner, PageLayout, QuotedComment, ShowMoreTextTextField } from '~/components';
import { STATUS_TEXT_MAPING } from '~/utils';

import { PartnerOrderItems } from './PartnerOrderItems';

dayjs.extend(LocalizedFormat);

const StatusBox = styled(Box)({
  fontWeight: 'bold',
  padding: '8px',
  position: 'absolute',
  right: '28px',
  top: '28px',
});

const StyledPaper = styled(Paper)({
  padding: '28px 25px',
  position: 'relative',
});

const variantMapping = { body1: 'div' };

const PartnerOrder = () => {
  const { orderId = '' } = useParams();
  const { data, isFetching } = useGetSellerOrderById(orderId);

  if (isFetching) {
    return <LoadingSpinner />;
  }

  return data ? (
    <PageLayout title={`Заказ №${data.number}`}>
      <Grid container>
        <Grid item lg={6} marginBottom={{ xs: 2 }} md={6} paddingRight={2} xs={12}>
          <StyledPaper>
            <StatusBox>{STATUS_TEXT_MAPING[data.state] ?? ''}</StatusBox>
            <Typography variant="h6">Основная информация</Typography>

            <Box paddingTop={2}>
              <Typography paddingBottom={1} variant="body1">
                Время создания: {dayjs(data.created).format('LLL')}
              </Typography>

              {data.paymentDate && (
                <Typography paddingBottom={1} variant="body1">
                  Время оплаты: {dayjs(data.paymentDate).format('LLL')}
                </Typography>
              )}

              {data.deliveredDate && (
                <Typography paddingBottom={1} variant="body1">
                  Время получения: {dayjs(data.deliveredDate).format('LLL')}
                </Typography>
              )}

              <Box color="text.lightGrey">
                {data.recipientAddress.country} {data.recipientAddress.city} {data.recipientAddress.street}{' '}
                {data.recipientAddress.entrance} {data.recipientAddress.apartmentNumber}
              </Box>

              {data.comments && (
                <QuotedComment mt={2} variantMapping={variantMapping}>
                  <ShowMoreTextTextField>{data.comments}</ShowMoreTextTextField>
                </QuotedComment>
              )}
            </Box>
          </StyledPaper>
        </Grid>

        <Grid item lg={6} md={6} paddingRight={2} xs={12}>
          <StyledPaper>
            <Typography variant="h6">Покупатель: {data.buyer.userName}</Typography>

            <Typography paddingBottom={1} variant="body1">
              {data.recipientPhone}
            </Typography>

            <PartnerOrderItems items={data.items as any} />
          </StyledPaper>
        </Grid>
      </Grid>
    </PageLayout>
  ) : (
    <></>
  );
};

export { PartnerOrder };
