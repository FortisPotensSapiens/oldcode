import { LoadingButton } from '@mui/lab';
import { Alert, Box, Button, Divider, Grid, styled, Typography } from '@mui/material';
import { Spin } from 'antd';
import { format } from 'date-fns';
import { FC, Fragment, useMemo } from 'react';
import { UseQueryResult } from 'react-query';
import { Link } from 'react-router-dom';

import { useGetPartnerById } from '~/api';
import {
  Error,
  LoadingSpinner,
  PageLayout,
  QuotedComment,
  ShowMoreTextTextField,
  SupportButton,
  TelegramButtonLink,
} from '~/components';
import { useCurrencySymbol } from '~/hooks';
import { SummServingSize } from '~/pages/PlaceIndividualOrder/IndividualOrderDetail/SummServingSize';
import { generateIndividualOrderOfferPath, generateProductPath, generateRepeatOrderPath, getCartPath } from '~/routing';
import { OrderDeliveryType, OrderItemType, OrderReadModel, OrderState, OrderType } from '~/types';
import { addressToString, getOrderStateDecoration } from '~/utils';
import { getOrderType } from '~/utils/getOrderType';
import { getSummServingSize } from '~/utils/getSumServingSize';

import { Pickup } from '../NewOrderView/OrderForm/Pickup';
import { StyledPaper } from './StyledPaper';
import { usePayOrder } from './usePayOrder';

type OrderViewProps = UseQueryResult<OrderReadModel>;

const variantMapping = { body1: 'div' };

const StyledIframe = styled('iframe')`
  border: 0;
`;

const OrderView: FC<OrderViewProps> = ({ data, isError, isLoading }) => {
  const currency = useCurrencySymbol();
  const [payOrderHandler, payOrder] = usePayOrder(data?.id);
  const servingSumm = useMemo(() => getSummServingSize(data?.items), [data?.items]);
  const { data: seller, isLoading: isSellerLoading } = useGetPartnerById(String(data?.seller.id));

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (isError || !data || !data.items) {
    return <Error />;
  }

  const { severity, text } = getOrderStateDecoration(data.state);
  const repeatPath = generateRepeatOrderPath({ orderId: data.id });

  return (
    <PageLayout href={getCartPath()} maxWidth={1156} title={`Информация о заказе №${data.number}`}>
      <Grid alignItems="flex-start" columnSpacing={3} container>
        <Grid container item md={8} rowSpacing={3} xs={12}>
          <Grid item order={{ sm: 1, xs: 2 }} xs={12}>
            <StyledPaper>
              <Grid alignItems="center" columnSpacing={3} container>
                <Grid item xs>
                  <Typography fontWeight={500} variant="h5">
                    Основная информация
                  </Typography>
                </Grid>

                <Grid item xs={12} sm>
                  <Box paddingRight={2} color={getOrderType(data.type).textColor} component={Box} whiteSpace="nowrap">
                    {getOrderType(data.type).text}
                  </Box>
                </Grid>

                <Grid item whiteSpace="nowrap" xs="auto">
                  <Alert icon={false} severity={severity}>
                    {text}
                  </Alert>
                </Grid>
              </Grid>

              <Typography mt={2}>{format(new Date(data.created), 'dd.MM.yyyy')}</Typography>

              <Typography color="text.lightGrey" mt={2}>
                {data.deliveryType === OrderDeliveryType.SelfDelivered
                  ? 'Самовывоз'
                  : addressToString(data.recipientAddress)}
              </Typography>

              {data.comments && (
                <QuotedComment mt={2} variantMapping={variantMapping}>
                  <ShowMoreTextTextField>{data.comments}</ShowMoreTextTextField>
                </QuotedComment>
              )}
            </StyledPaper>
          </Grid>

          {data.deliveryType === OrderDeliveryType.SelfDelivered ? (
            <Grid item order={{ sm: 2, xs: 3 }} xs={12} marginTop={2}>
              <StyledPaper>
                {isSellerLoading ? <Spin /> : undefined}
                {seller ? (
                  <>
                    <Typography marginBottom={2} fontWeight={500} variant="h5">
                      Контакты магазина
                    </Typography>
                    <Pickup seller={seller} showPhone />
                  </>
                ) : undefined}
              </StyledPaper>
            </Grid>
          ) : undefined}

          {data.deliveryInfo?.buyerDeliveryTrackingUrl ? (
            <Grid xs={12} marginTop={2} marginBottom={2} order={{ sm: 1, xs: 2 }}>
              <StyledPaper>
                <StyledIframe
                  title="map"
                  width="100%"
                  height="600px"
                  src={data.deliveryInfo.buyerDeliveryTrackingUrl}
                />
              </StyledPaper>
            </Grid>
          ) : undefined}
        </Grid>

        <Grid item md={4} xs={12}>
          <StyledPaper>
            <Typography fontWeight={500} mb={3} variant="h6">
              Продавец: {data.seller.title}
            </Typography>

            <Divider />

            <Box columnGap={4} display="grid" gridTemplateColumns="1fr auto auto" my={3} rowGap={2} width={1}>
              {data.items.map((item) => (
                <Fragment key={item.itemId ?? item.offerId}>
                  <Box>
                    {String(data.type) === String(OrderItemType.Standard) ? (
                      <Link
                        to={generateProductPath({
                          productId: String(item.itemId),
                        })}
                        style={{
                          color: '#000',
                        }}
                      >
                        {item.title}
                      </Link>
                    ) : (
                      <Link
                        to={generateIndividualOrderOfferPath(item.orderId, String(item.offerId))}
                        style={{
                          color: '#000',
                        }}
                      >
                        {item.title}
                      </Link>
                    )}
                  </Box>

                  <Box color="text.lightGrey" whiteSpace="nowrap">
                    {item.amount.toLocaleString()} шт.
                  </Box>

                  <Box textAlign="right" whiteSpace="nowrap">
                    {(item.amount * (item.price ?? 0)).toLocaleString()} {currency}
                  </Box>
                </Fragment>
              ))}
            </Box>

            <Divider />

            <Typography display="flex" fontWeight={500} justifyContent="space-between" my={3} variant="h6">
              Итого&nbsp;
              <span>
                {data.amount.toLocaleString()} {currency}
              </span>
            </Typography>

            <SummServingSize size={servingSumm} />

            {data.state === OrderState.Paid && data.type === OrderType.Standard ? (
              <Button component={Link} fullWidth size="large" to={repeatPath} variant="contained">
                Заказать снова
              </Button>
            ) : undefined}

            {data.state === OrderState.Created ? (
              <Grid container spacing={3}>
                <Grid item sm={6} xs={12}>
                  <LoadingButton
                    fullWidth
                    loading={payOrder.isLoading || payOrder.isSuccess}
                    onClick={payOrderHandler}
                    size="large"
                    variant="contained"
                  >
                    Оплатить
                  </LoadingButton>
                </Grid>

                {String(data.type) === String(OrderItemType.Standard) ? (
                  <Grid item sm={6} xs={12}>
                    <Button component={Link} fullWidth size="large" to={repeatPath} variant="outlined">
                      Повторить заказ
                    </Button>
                  </Grid>
                ) : undefined}
              </Grid>
            ) : undefined}
          </StyledPaper>

          <Grid item order={{ sm: 2, xs: 1 }} xs={12} marginTop={2}>
            <StyledPaper>
              <Typography fontWeight={500} variant="h5">
                Поддержка
              </Typography>

              <Typography mb={1} mt={2}>
                Если у вас есть вопросы или вы испытываете проблемы с сервисом напишите нам.
              </Typography>

              <Grid columnSpacing={3} container>
                <Grid item sm="auto" xs={6} marginBottom={2}>
                  <SupportButton />
                </Grid>

                <Grid item sm xs={6}>
                  <TelegramButtonLink />
                </Grid>
              </Grid>
            </StyledPaper>
          </Grid>
        </Grid>
      </Grid>
    </PageLayout>
  );
};

export { OrderView };
export type { OrderViewProps };
