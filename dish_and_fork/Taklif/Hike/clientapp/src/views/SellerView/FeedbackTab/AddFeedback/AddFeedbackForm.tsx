import { yupResolver } from '@hookform/resolvers/yup';
import { Button, Col, Rate, Row, Typography } from 'antd';
import TextArea from 'antd/es/input/TextArea';
import Title from 'antd/es/typography/Title';
import { useEffect } from 'react';
import { Controller, FormProvider, useForm, useFormState } from 'react-hook-form';
import * as yup from 'yup';

import { MerchRatingCreateModel } from '~/types';

const validationSchema = yup.object({
  rating: yup.number().required(),
});

const AddFeedbackForm = ({
  productId,
  onSubmit,
  loading = false,
  defaultValues = null,
  submitButtonText = 'Оставить отзыв',
}: {
  productId: string;
  onSubmit: (data: MerchRatingCreateModel) => void;
  loading: boolean;
  defaultValues?: MerchRatingCreateModel | null;
  submitButtonText?: string;
}) => {
  const { handleSubmit, watch, ...methods } = useForm<MerchRatingCreateModel>({
    defaultValues: { ...(defaultValues ?? {}), merchId: productId },
    mode: 'all',
    resolver: yupResolver(validationSchema),
  });
  const { isValid } = useFormState({ control: methods.control });

  useEffect(() => {
    if (defaultValues) {
      methods.reset(defaultValues);
    }
  }, [defaultValues]);

  return (
    <FormProvider handleSubmit={handleSubmit} watch={watch} {...methods}>
      <Row justify="start" gutter={[0, 15]}>
        <Col span={24}>
          <Typography>
            <Title level={4}>Мой отзыв</Title>
          </Typography>
        </Col>
        <Col span={24}>
          <Controller name="rating" render={({ field }) => <Rate style={{ color: '#7e004a' }} {...field} />} />
        </Col>
        <Col span={24}>
          <Controller
            name="comment"
            render={({ field }) => <TextArea placeholder="Комментарий" rows={3} {...field} />}
          />
        </Col>
        <Col span={24}>
          <Button loading={loading} type="primary" onClick={handleSubmit(onSubmit)} disabled={!isValid}>
            {submitButtonText}
          </Button>
        </Col>
      </Row>
    </FormProvider>
  );
};

export default AddFeedbackForm;
