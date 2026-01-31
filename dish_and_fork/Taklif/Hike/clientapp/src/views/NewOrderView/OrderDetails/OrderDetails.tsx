import styled from '@emotion/styled';
import { Box, Divider, Typography } from '@mui/material';
import { Alert, Spin } from 'antd';
import { isNumber } from 'lodash-es';
import { FC } from 'react';

import { useCurrencySymbol } from '~/hooks';
import { CartItem } from '~/types';
import { pluralize } from '~/utils';

import { ORDER_VARIANT_SHIPPING, OrderVariant } from '../OrderForm';
import { StyledPaper } from './StyledPaper';
import { useOrderSummary } from './useOrderSummary';

const StyledAlert = styled(Alert)`
  margin-top: 1rem;
`;

type OrderDetailsProps = {
  items: CartItem[];
  variant: OrderVariant;
  deliveryCost?: number;
  deliveryCostFetching?: boolean;
  isDeliveryCalculatingError?: boolean;
  deliveryCostFetched?: boolean;
};

const OrderDetails: FC<OrderDetailsProps> = ({
  children,
  items,
  variant,
  deliveryCost,
  deliveryCostFetching,
  isDeliveryCalculatingError,
  deliveryCostFetched,
}) => {
  const { count, sum } = useOrderSummary(items);
  const currency = useCurrencySymbol();

  return (
    <Box borderRadius={2.5} component={StyledPaper} px={{ sm: 3, xs: 0 }} py={{ sm: 3, xs: 1 }} width={1}>
      <Box display="grid" gridTemplateColumns="1fr auto" sx={{ columnGap: 1 }}>
        <Typography variant="h4">Ваш заказ</Typography>

        <Box alignSelf="flex-end" color="text.secondary" typography="body1">
          {count} {pluralize(count, 'товаров', 'товар', 'товара', 'товаров')}
        </Box>
      </Box>

      <Box component="ul" m={0} p={0} sx={{ listStyleType: 'none' }}>
        {items.map(({ amount, merchandise }) => (
          <Box key={merchandise.id} component="li" display="flex" mt={2}>
            <Box flexGrow={1}>{merchandise.title}</Box>

            <Box ml={1} whiteSpace="nowrap">
              {(amount * merchandise.price).toLocaleString()} {currency}
            </Box>
          </Box>
        ))}
      </Box>

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
        <span>{(sum + (variant === ORDER_VARIANT_SHIPPING ? deliveryCost ?? 0 : 0)).toLocaleString()} ₽</span>
      </Box>

      {children}
    </Box>
  );
};

export { OrderDetails };
