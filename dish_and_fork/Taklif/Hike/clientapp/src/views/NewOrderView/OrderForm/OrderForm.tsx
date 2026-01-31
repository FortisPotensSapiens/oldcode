import { TabContext, TabList, TabPanel } from '@mui/lab';
import { Box, Grid, Tab } from '@mui/material';
import { Input } from 'antd';
import { FC } from 'react';
import { Controller } from 'react-hook-form';

import { LoadingSpinner } from '~/components';
import FloatLabel from '~/components/FloatLabel/FloatLabel';
import { FormFieldPhone } from '~/components/FormFieldPhone/AntFormFieldPhone';
import { PartnerReadModel } from '~/types/swagger';

import { AddressForm } from './AddressForm';
import { ORDER_VARIANT_PICKUP, ORDER_VARIANT_SHIPPING } from './constants';
import { Pickup } from './Pickup';
import { OrderVariant } from './types';
import { useTabs } from './useTabs';

type OrderFormProps = {
  seller?: PartnerReadModel;
  variant?: OrderVariant;
};

const required = 'Заполните поле';

const OrderForm: FC<OrderFormProps> = ({ seller, variant = ORDER_VARIANT_SHIPPING }) => {
  const [value, change] = useTabs(variant);

  return (
    <Grid container spacing={3}>
      <Grid container item spacing={3}>
        <Grid item xs={12}>
          <Controller
            name="recipientFullName"
            render={({ field }) => (
              <FloatLabel label="Имя получателя" value={field.value}>
                <Input {...field} />
              </FloatLabel>
            )}
          />
        </Grid>

        <Grid item xs={12}>
          <FormFieldPhone label="Номер телефона получателя" name="recipientPhone" />
        </Grid>
      </Grid>

      <Grid item xs={12}>
        <TabContext value={value}>
          <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
            <TabList aria-label="order contacts" onChange={change} variant="fullWidth">
              <Tab label="Доставка" value={ORDER_VARIANT_SHIPPING} />
              {seller?.isPickupEnabled ? <Tab label="Самовывоз" value={ORDER_VARIANT_PICKUP} /> : undefined}
            </TabList>
          </Box>

          <Box component={TabPanel} px={0} value={ORDER_VARIANT_SHIPPING}>
            <Grid container spacing={3}>
              <Grid item xs={12}>
                <AddressForm required={required} />
              </Grid>

              <Grid item xs={12}>
                <Controller
                  name="comments"
                  render={({ field }) => (
                    <FloatLabel label="Комментарий курьеру" value={field.value}>
                      <Input.TextArea {...field} autoSize />
                    </FloatLabel>
                  )}
                />
              </Grid>
            </Grid>
          </Box>

          <Box component={TabPanel} px={0} value={ORDER_VARIANT_PICKUP}>
            {!seller ? (
              <LoadingSpinner />
            ) : seller?.address ? (
              <Pickup seller={seller} />
            ) : (
              <Box my={3} textAlign="center">
                Самовывоз невозможен
              </Box>
            )}
          </Box>
        </TabContext>
      </Grid>
    </Grid>
  );
};

export { OrderForm };
