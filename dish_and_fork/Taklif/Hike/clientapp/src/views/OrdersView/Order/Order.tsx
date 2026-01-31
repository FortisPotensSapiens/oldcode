import { LoadingButton } from '@mui/lab';
import { Alert, Box, Button, Grid, Paper, Typography } from '@mui/material';
import { format } from 'date-fns';
import { FC } from 'react';

import { EllipsisBox } from '~/components';
import { useCurrencySymbol } from '~/hooks';
import { generateOrderPath } from '~/routing';
import { OrderDeliveryType, OrderItemType, OrderReadModel, OrderState } from '~/types';
import { addressToString, getOrderStateDecoration } from '~/utils';
import { getOrderType } from '~/utils/getOrderType';

import { StyledLink } from './StyledLink';
import { useClickButton } from './useClickButton';
import { useNavigateOrder } from './useNavigateOrder';
import { usePay } from './usePay';
import { useRepeat } from './useRepeat';

type OrderProps = OrderReadModel & {
  disabled?: boolean;
  onPay: (payload: boolean, orderId: string) => void;
};

const Order: FC<OrderProps> = (props) => {
  const { amount, created, deliveryType, disabled, id, number, onPay, recipientAddress, state, type } = props;
  // TODO добавить обработку ошибки
  const currency = useCurrencySymbol();
  const to = generateOrderPath({ orderId: id });
  const [pay, { isLoading }] = usePay(id, onPay);
  const repeat = useRepeat(id);
  const navigate = useNavigateOrder(to);
  const repeatHandler = useClickButton(repeat);
  const payHandler = useClickButton(pay);

  const title = (
    <Typography color="text.primary" variant="h6">
      № {number}
    </Typography>
  );

  const { severity, text } = getOrderStateDecoration(state);

  return (
    <Grid item md={6} xs={12}>
      <Box
        borderRadius={{ sm: 2.5, xs: 0 }}
        component={Paper}
        onClick={disabled ? undefined : navigate}
        p={3}
        sx={{ '&:hover': { cursor: 'pointer' } }}
      >
        <Box alignItems="center" display="flex">
          <Grid container alignItems="center">
            {disabled ? (
              title
            ) : (
              <Grid item xs>
                <StyledLink to={to}>{title}</StyledLink>
              </Grid>
            )}

            <Grid item xs>
              <Box color="text.lightGrey" flexGrow={1} mx={2} whiteSpace="nowrap">
                от {format(new Date(created), 'dd.MM.yyyy')}
              </Box>
            </Grid>

            <Grid item xs={12} sm>
              <Box paddingRight={2} color={getOrderType(type).textColor} component={Box} whiteSpace="nowrap">
                {getOrderType(type).text}
              </Box>
            </Grid>

            <Grid item>
              <Box component={Alert} icon={false} severity={severity} whiteSpace="nowrap">
                {text}
              </Box>
            </Grid>
          </Grid>
        </Box>

        <EllipsisBox color="text.lightGrey" mt={2}>
          {deliveryType === OrderDeliveryType.SelfDelivered ? 'Самовывоз' : addressToString(recipientAddress)}
        </EllipsisBox>

        <Box alignItems="center" display="flex" mt={2.5}>
          <Box flexGrow={1} fontSize="1.5rem">
            {amount.toLocaleString()} {currency}
          </Box>

          {state === OrderState.Created ? (
            <LoadingButton
              disabled={disabled}
              loading={isLoading}
              onClick={payHandler}
              size="large"
              variant="contained"
            >
              Оплатить
            </LoadingButton>
          ) : undefined}

          {state === OrderState.Paid && String(type) === String(OrderItemType.Standard) ? (
            <Button onClick={repeatHandler} size="large" variant="outlined">
              Повторить заказ
            </Button>
          ) : undefined}
        </Box>
      </Box>
    </Grid>
  );
};

export { Order };
