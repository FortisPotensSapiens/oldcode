// eslint-disable-next-line simple-import-sort/imports
import { yupResolver } from '@hookform/resolvers/yup';
import { Box, Button, Grid } from '@mui/material';
import { useSnackbar } from 'notistack';
import { FC, useEffect, useMemo } from 'react';
import { FormProvider, useForm, useFormState } from 'react-hook-form';
import * as yup from 'yup';

import { useDebounce } from 'usehooks-ts';
import { useGetPartnerById } from '~/api';
import { EmptyList, PageLayout } from '~/components';
import { getCartPath } from '~/routing';
import { CartItem, LastOrderInfo } from '~/types';

import { OrderDetails } from './OrderDetails';
import { ORDER_VARIANT_SHIPPING, OrderForm } from './OrderForm';
import { FormData } from './types';
import { usePlaceAndPay } from './usePlaceAndPay';

import 'yup-phone-lite';
import { usePostDeliveryStandarOrder } from '~/api/v1/usePostDeliveryStandarOrder';

type NewOrderViewProps = Omit<LastOrderInfo, 'items'> & {
  clearCart?: boolean;
  items: CartItem[];
  sellerId: string;
};

const validationSchema = yup.object({
  recipientPhone: yup.string().phone().required(),
});

const NewOrderView: FC<NewOrderViewProps> = ({ children, clearCart, items, sellerId, ...props }) => {
  const { enqueueSnackbar } = useSnackbar();

  const { apartmentNumber, comments, entrance, house, intercom, recipientFullName, recipientPhone, street } = props;

  const defaultValues: FormData = {
    apartmentNumber: apartmentNumber ?? '',
    comments: comments ?? '',
    entrance: entrance ?? '',
    house: house ?? '',
    intercom: intercom ?? '',
    recipientFullName: recipientFullName ?? '',
    recipientPhone: recipientPhone ?? '',
    street: street ?? '',
    variant: ORDER_VARIANT_SHIPPING,
  };

  const { handleSubmit, watch, ...methods } = useForm({
    defaultValues,
    mode: 'all',
    resolver: yupResolver(validationSchema),
  });
  const { isValid } = useFormState({ control: methods.control });
  const { data, isError, isLoading } = useGetPartnerById(sellerId);
  const noData = items.length === 0 && !isLoading && !isError;

  const variant = watch('variant');
  const debouncedStreet = useDebounce(watch('street'), 1000);
  const debouncedHouse = useDebounce(watch('house'), 1000);

  const [send, { error, isLoading: isSending }] = usePlaceAndPay(items, clearCart ?? true);

  const itemsCalculate = useMemo(() => {
    return items.map((item) => {
      return {
        itemId: item.merchandise.id,
        amount: item.amount,
      };
    });
  }, [items]);

  const deliveryCalculate = usePostDeliveryStandarOrder({
    items: itemsCalculate,
    recipientAddress: {
      street: debouncedStreet,
      house: debouncedHouse,
      latitude: 0,
      longitude: 0,
      zipCode: '0',
    },
  });

  const isDeliveryCalculatingError = useMemo(() => {
    return deliveryCalculate.isError && variant === ORDER_VARIANT_SHIPPING;
  }, [deliveryCalculate.isError, variant]);

  useEffect(() => {
    if (error) {
      // eslint-disable-next-line
      enqueueSnackbar(Object.values((error as any)?.response?.data?.errors).join('\n'), {
        variant: 'error',
      });
    }
  }, [enqueueSnackbar, error]);

  return (
    <PageLayout href={getCartPath()} maxWidth={1156} title="Оформление заказа">
      {noData ? (
        <EmptyList>Ваша корзина пуста</EmptyList>
      ) : (
        <FormProvider handleSubmit={handleSubmit} watch={watch} {...methods}>
          <Box component="form" mx={{ sm: 0, xs: 2 }} onSubmit={handleSubmit(send)}>
            <Grid alignItems="flex-start" columnSpacing={3} container mt={3}>
              <Grid container item md={6} xs={12}>
                <OrderForm seller={data} variant={defaultValues.variant} />
              </Grid>

              <Grid container item md={6} xs={12}>
                <Box width={1}>
                  <OrderDetails
                    items={items}
                    variant={variant}
                    deliveryCost={deliveryCalculate.data?.price}
                    deliveryCostFetching={deliveryCalculate.isFetching}
                    deliveryCostFetched={deliveryCalculate.isFetched}
                    isDeliveryCalculatingError={isDeliveryCalculatingError}
                  >
                    <Button
                      disabled={isLoading || isError || isSending || !isValid || isDeliveryCalculatingError}
                      fullWidth
                      type="submit"
                      variant="contained"
                    >
                      Оплатить
                    </Button>
                  </OrderDetails>
                </Box>
              </Grid>
            </Grid>
          </Box>
        </FormProvider>
      )}
    </PageLayout>
  );
};

export { NewOrderView };
export type { NewOrderViewProps };
