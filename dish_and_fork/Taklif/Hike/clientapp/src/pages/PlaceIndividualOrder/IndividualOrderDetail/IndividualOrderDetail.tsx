import { Box, Divider, Grid, Paper, styled, Typography } from '@mui/material';
import { Alert, Spin } from 'antd';
import dayjs from 'dayjs';
import LocalizedFormat from 'dayjs/plugin/localizedFormat';
import timezone from 'dayjs/plugin/timezone';
import utc from 'dayjs/plugin/utc';
import { isNumber } from 'lodash-es';
import { FC } from 'react';

import { useCurrencySymbol } from '~/hooks';
import { StyledShowMoreTextTextField } from '~/pages/IndividualOrder/IndividualOrderResponses/IndividualOrderResponsesItem/styled';
import { ApplicationDetailsReadModel, OfferDetailsReamModel } from '~/types';
import { getCurrencySymbol } from '~/utils';
import { ORDER_VARIANT_SHIPPING, OrderVariant } from '~/views/NewOrderView/OrderForm';

import { SummServingSize } from './SummServingSize';

dayjs.extend(LocalizedFormat);
dayjs.extend(utc);
dayjs.extend(timezone);

const StyledValue = styled(Box)(({ theme }) => ({
  display: 'inline-block',
  fontWeight: 'bold',
  paddingLeft: theme.spacing(2),
}));

const StyledTitle = styled(Box)(({ theme }) => ({
  fontWeight: 'bold',
  paddingBottom: theme.spacing(1),
}));

const StyledPaper = styled(Paper, { name: 'StyledPaper' })(({ theme }) => ({
  [theme.breakpoints.down('sm')]: {
    backgroundColor: 'transparent',
    borderWidth: 0,
    boxShadow: 'none',
  },
}));

const StyledAlert = styled(Alert)`
  margin-top: 1rem;
  margin-bottom: 1rem;
`;

const IndividualOrderDetail: FC<{
  offerData: OfferDetailsReamModel;
  individualOrderData: ApplicationDetailsReadModel;
  variant: OrderVariant;
  deliveryCost?: number;
  deliveryCostFetching?: boolean;
  isDeliveryCalculatingError?: boolean;
  deliveryCostFetched?: boolean;
}> = ({
  offerData,
  individualOrderData,
  children,
  variant,
  deliveryCost,
  deliveryCostFetching,
  isDeliveryCalculatingError,
  deliveryCostFetched,
}) => {
  const currency = useCurrencySymbol();

  return (
    <Box component={StyledPaper} borderRadius={2.5} px={{ sm: 3, xs: 0 }} py={{ sm: 3, xs: 1 }} width={1}>
      <Box display="grid" gridTemplateColumns="1fr auto" sx={{ columnGap: 1 }}>
        <Typography variant="h4">Ваш заказ: {individualOrderData.title}</Typography>
      </Box>

      <Grid container rowSpacing={2} margin={2}>
        <Grid item sm={8} xs={12}>
          Цена
        </Grid>
        <Grid item sm={4} xs={12}>
          <StyledValue>
            {offerData?.sum} {getCurrencySymbol('Rub')}
          </StyledValue>
        </Grid>
        <Grid item sm={8} xs={12}>
          Дата приготовления
        </Grid>
        <Grid item sm={4} xs={12}>
          <StyledValue>{dayjs(offerData?.date).local().format('L')}</StyledValue>
        </Grid>
        <Grid item xs={12}>
          <StyledTitle>Комментарий продавца</StyledTitle>
          <StyledShowMoreTextTextField>{offerData?.description}</StyledShowMoreTextTextField>
        </Grid>
      </Grid>

      {variant === ORDER_VARIANT_SHIPPING ? (
        <>
          {isDeliveryCalculatingError ? (
            <StyledAlert message="Для расчета стоимости доставки укажите пожалуйста Ваш адрес" type="error" />
          ) : (
            <>
              <Box component={Divider} mx={-3} my={3} />

              <Box component="ul" m={0} p={0} sx={{ listStyleType: 'none' }}>
                <Box component="li" display="flex" mt={2}>
                  <Box flexGrow={1}>Стоимость доставки</Box>
                  <Box ml={1} whiteSpace="nowrap">
                    {deliveryCostFetching ? <Spin /> : undefined}

                    {isNumber(deliveryCost) && deliveryCostFetched ? (
                      <>
                        {deliveryCost} {currency}
                      </>
                    ) : undefined}
                  </Box>
                </Box>
              </Box>
            </>
          )}
        </>
      ) : undefined}

      <Box component={Divider} mx={-3} my={3} />

      <Box display="flex" justifyContent="space-between" mb={3} typography="h4">
        <span>Итого</span>
        <span>{(offerData.sum + (variant === ORDER_VARIANT_SHIPPING ? deliveryCost ?? 0 : 0)).toLocaleString()} ₽</span>
      </Box>

      <SummServingSize size={offerData.servingGrossWeightInKilograms} />

      {children}
    </Box>
  );
};

export default IndividualOrderDetail;
