import { ErrorMessage } from '@hookform/error-message';
import { yupResolver } from '@hookform/resolvers/yup';
import { Alert, Col, Form, Input, Modal, Row, Select, Spin } from 'antd';
import { useMemo } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';
import * as yup from 'yup';

import { CONFIG_ENUM_KEYS, useGetConfigEnums } from '~/api/v1/useGetConfigEnums';
import { CategoryCreateModel, CategoryUpdateModel } from '~/types';

const validationSchema = yup.object({
  title: yup.string().required(),
  type: yup.string().required(),
});

const AdminTagsAddModal = ({
  open = false,
  tagId = null,
  title,
  onClose,
  onSuccess,
  defaultValues,
  isPending,
}: {
  open: boolean;
  title: string;
  defaultValues?: CategoryCreateModel;
  tagId?: string | null;
  onClose: () => void;
  onSuccess: (data: CategoryCreateModel | CategoryUpdateModel) => void;
  isPending: boolean;
}) => {
  const { data } = useGetConfigEnums();
  const { t } = useTranslation();
  const form = useForm<CategoryCreateModel | CategoryUpdateModel>({
    mode: 'all',
    defaultValues,
    resolver: yupResolver(validationSchema),
  });
  const kinds = useMemo(() => {
    return data?.[CONFIG_ENUM_KEYS.CategoryType] ?? [];
  }, [data]);

  const onOk = () => {
    const data: CategoryCreateModel | CategoryUpdateModel = {
      ...form.getValues(),
    };

    if (tagId) {
      (data as CategoryUpdateModel).id = tagId;
    }

    onSuccess(data);
  };

  return (
    <Modal
      title={title}
      open={open}
      onCancel={onClose}
      onOk={onOk}
      okText="Применить"
      cancelText="Отменить"
      okButtonProps={{ disabled: !form.formState.isValid || isPending }}
    >
      <Form name="basic" layout="vertical">
        <Form.Item label="Заголовок" required>
          <Controller
            control={form.control}
            name="title"
            render={({ field }) => <Input placeholder="" {...field} disabled={isPending} />}
            rules={{ required: true }}
          />

          <ErrorMessage
            errors={form.formState.errors}
            name="title"
            render={({ message }) => <Alert message={message} type="error" />}
          />
        </Form.Item>

        <Form.Item label="Тип тега" required>
          <Controller
            control={form.control}
            name="type"
            render={({ field }) => (
              <Select style={{ width: '100%' }} {...field} disabled={isPending}>
                {kinds.map((item) => {
                  return <Select.Option key={item.name}>{t(`enums.type.${item.name}`)}</Select.Option>;
                })}
              </Select>
            )}
            rules={{ required: true }}
          />
          <ErrorMessage
            errors={form.formState.errors}
            name="type"
            render={({ message }) => <Alert message={message} type="error" />}
          />
        </Form.Item>

        {isPending ? (
          <Row justify="center" align="top">
            <Col>
              <Spin />
            </Col>
          </Row>
        ) : undefined}
      </Form>
    </Modal>
  );
};

export default AdminTagsAddModal;
