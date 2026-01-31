import { Col, Input, Row } from 'antd';
import { FC } from 'react';
import { Controller } from 'react-hook-form';

import FloatLabel from '~/components/FloatLabel/FloatLabel';

import { FieldProps } from './Field';

type AddressFormProps = Pick<FieldProps, 'required'>;

const AddressForm: FC<AddressFormProps> = ({ required }) => {
  return (
    <>
      <Row gutter={[16, 16]}>
        <Col span={24}>
          <Controller
            name="street"
            render={({ field }) => (
              <FloatLabel label="Улица" required value={field.value}>
                <Input {...field} />
              </FloatLabel>
            )}
            rules={{ required }}
          />
        </Col>
        <Col sm={6} xs={24}>
          <Controller
            name="house"
            render={({ field }) => (
              <FloatLabel label="Дом" required value={field.value}>
                <Input {...field} />
              </FloatLabel>
            )}
            rules={{ required }}
          />
        </Col>

        <Col sm={6} xs={24}>
          <Controller
            name="apartmentNumber"
            render={({ field }) => (
              <FloatLabel label="Кв/Офис" value={field.value}>
                <Input {...field} />
              </FloatLabel>
            )}
          />
        </Col>

        <Col sm={6} xs={24}>
          <Controller
            name="intercom"
            render={({ field }) => (
              <FloatLabel label="Домофон" value={field.value}>
                <Input {...field} />
              </FloatLabel>
            )}
          />
        </Col>

        <Col sm={6} xs={24}>
          <Controller
            name="entrance"
            render={({ field }) => (
              <FloatLabel label="Подъезд" value={field.value}>
                <Input {...field} />
              </FloatLabel>
            )}
          />
        </Col>
      </Row>
    </>
  );
};

export { AddressForm };
export type { AddressFormProps };
