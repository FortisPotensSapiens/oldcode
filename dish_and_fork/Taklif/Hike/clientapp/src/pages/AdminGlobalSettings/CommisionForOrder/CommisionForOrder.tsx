import { ErrorMessage } from '@hookform/error-message';
import { Alert, Form, Input, Typography } from 'antd';
import { FC } from 'react';
import { Controller } from 'react-hook-form';

const CommisionForOrder: FC = () => {
  return (
    <>
      <Typography.Title level={4}>Комиссия за товар</Typography.Title>
      <Form.Item label="Минимальная сумма комиссии" required>
        <Controller
          name="minMerchPrice"
          render={({ field }) => <Input placeholder="" {...field} disabled />}
          rules={{ required: true }}
        />

        <ErrorMessage name="minMerchPrice" render={({ message }) => <Alert message={message} type="error" />} />
      </Form.Item>

      <Form.Item label="Процент коммиссии" required>
        <Controller
          name="merchCoefficient"
          render={({ field }) => <Input placeholder="" {...field} />}
          rules={{ required: true }}
        />
        <ErrorMessage name="merchCoefficient" render={({ message }) => <Alert message={message} type="error" />} />
      </Form.Item>
    </>
  );
};

export { CommisionForOrder };
