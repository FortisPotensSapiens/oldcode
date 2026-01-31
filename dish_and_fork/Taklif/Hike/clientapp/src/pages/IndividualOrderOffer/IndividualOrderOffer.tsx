import { Box, Breadcrumbs, Button, Grid, Paper, styled, Typography } from '@mui/material';
import dayjs from 'dayjs';
import LocalizedFormat from 'dayjs/plugin/localizedFormat';
import timezone from 'dayjs/plugin/timezone';
import utc from 'dayjs/plugin/utc';
import { useNavigate, useParams } from 'react-router-dom';

import { useGetMyUserProfile, useGetPartnerMy } from '~/api';
import { useGetIndividualOrder } from '~/api/v1/useGetIndividualOrder';
import { useGetIndividualOrderOfferInfo } from '~/api/v1/useGetIndividualOrderOfferInfo';
import { Link, LoadingSpinner, PageLayout } from '~/components';
import { useConfig } from '~/contexts';
import { generateIndividualOrderPath, generatePlaceIndividualOrderPath, getIndividualOrdersPath } from '~/routing';
import { FileReadModel } from '~/types';
import { getCurrencySymbol } from '~/utils';
import { Images } from '~/views/PartnerProductView/Images';

import IndividualOrderChat from '../IndividualOrder/IndividualOrderResponses/IndividualOrderChatDialog/IndividualOrderChat/IndividualOrderChat';
import { StyledShowMoreTextTextField } from '../IndividualOrder/IndividualOrderResponses/IndividualOrderResponsesItem/styled';

dayjs.extend(LocalizedFormat);
dayjs.extend(utc);
dayjs.extend(timezone);

const StyledValue = styled(Box)(({ theme }) => ({
  display: 'inline-block',
  fontWeight: 'bold',
  paddingLeft: theme.spacing(2),
}));

const StyledPaper = styled(Paper)(({ theme }) => ({
  padding: theme.spacing(2),
  marginTop: theme.spacing(2),
}));

const StyledText = styled(Typography)(() => ({
  opacity: '0.8',
}));

const IndividualOrderOffer = () => {
  const navigate = useNavigate();
  const { individualOrderOfferId, individualOrderId } = useParams();
  const { data, isLoading } = useGetIndividualOrderOfferInfo(String(individualOrderOfferId));
  const { data: individualOrder, isLoading: isLoadingOrder } = useGetIndividualOrder(String(individualOrderId));

  const { data: my } = useGetMyUserProfile();
  const { data: partnerMy } = useGetPartnerMy();
  const { roles } = useConfig();

  if (isLoading || isLoadingOrder) {
    return <LoadingSpinner />;
  }

  const onOrder = () => {
    navigate(generatePlaceIndividualOrderPath(String(individualOrderId), String(individualOrderOfferId)));
  };

  return (
    <>
      <Box display={{ sm: 'inline', xs: 'none' }}>
        <Breadcrumbs>
          <Link key="root" color="text.secondary" to={getIndividualOrdersPath()} sx={{ textDecoration: 'none' }}>
            Индивидуальные заказы
          </Link>

          <Link
            key="root"
            color="text.secondary"
            to={generateIndividualOrderPath(String(individualOrderId))}
            sx={{ textDecoration: 'none' }}
          >
            {individualOrder?.title}
          </Link>

          <Box key="title" color="text.primary">
            Предложение от магазина: {data?.seller.title}
          </Box>
        </Breadcrumbs>
      </Box>
      <PageLayout title={<>Предложение от магазина: {data?.seller.title}</>}>
        <Grid container rowSpacing={2} margin={2}>
          <Grid item sm={8} xs={12}>
            Номер предложения
          </Grid>
          <Grid item sm={4} xs={12}>
            <StyledValue>№{data?.number}</StyledValue>
          </Grid>
          <Grid item sm={8} xs={12}>
            Цена запрошенная продавцом магазина
          </Grid>
          <Grid item sm={4} xs={12}>
            <StyledValue>
              {data?.sum} {getCurrencySymbol('Rub')}
            </StyledValue>
          </Grid>
          <Grid item sm={8} xs={12}>
            Дата приготовления
          </Grid>
          <Grid item sm={4} xs={12}>
            <StyledValue>{dayjs(data?.date).local().format('L')}</StyledValue>
          </Grid>
          <Grid item xs={12}>
            <StyledText variant="body2">
              <StyledShowMoreTextTextField>{data?.description}</StyledShowMoreTextTextField>
            </StyledText>
          </Grid>

          {data?.images ? (
            <Grid item xs={12}>
              <Typography>Примеры работ: </Typography>
              <Images
                showPreview
                addabled={false}
                isDraggingEnabled={false}
                max={999}
                showCrow={false}
                cropping={false}
                value={data?.images as FileReadModel[]}
                containerProps={{
                  display: 'flex',
                  overflow: 'scroll',
                }}
                itemProps={{
                  paddingRight: 2,
                }}
              />
            </Grid>
          ) : undefined}

          {my?.id === individualOrder?.customer?.id && !individualOrder?.selectedOrderId ? (
            <Grid item xs={12}>
              <Button variant="outlined" onClick={onOrder}>
                Принять предложение
              </Button>
            </Grid>
          ) : undefined}
        </Grid>
        {!my?.roles?.includes(roles.seller) ||
        (my?.roles?.includes(roles.seller) && partnerMy?.id === data?.seller.id) ? (
          <StyledPaper>
            <IndividualOrderChat chatBodyHeight="300px" offerId={String(individualOrderOfferId)} />
          </StyledPaper>
        ) : undefined}
      </PageLayout>
    </>
  );
};

export default IndividualOrderOffer;
