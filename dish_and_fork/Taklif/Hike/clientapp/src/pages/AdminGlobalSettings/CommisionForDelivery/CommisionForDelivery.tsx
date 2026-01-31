import { ErrorMessage } from '@hookform/error-message';
import { Alert, Form, Input, Typography } from 'antd';
import { FC } from 'react';
import { Controller } from 'react-hook-form';

const CommisionForDelivery: FC = () => {
  return (
    <>
      <Typography.Title level={4}>Комиссия за доставку</Typography.Title>
      <Form.Item label="Минимальная сумма комиссии" required>
        <Controller
          name="minDeliveryPrice"
          render={({ field }) => <Input placeholder="" {...field} />}
          rules={{ required: true }}
        />

        <ErrorMessage name="minDeliveryPrice" render={({ message }) => <Alert message={message} type="error" />} />
      </Form.Item>

      <Form.Item label="Процент коммиссии" required>
        <Controller
          name="deliveryCoefficient"
          render={({ field }) => <Input placeholder="" {...field} />}
          rules={{ required: true }}
        />
        <ErrorMessage name="deliveryCoefficient" render={({ message }) => <Alert message={message} type="error" />} />
      </Form.Item>
    </>
  );
};

export { CommisionForDelivery };
