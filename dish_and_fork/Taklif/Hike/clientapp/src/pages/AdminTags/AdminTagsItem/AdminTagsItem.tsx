import { EyeInvisibleOutlined, EyeOutlined } from '@ant-design/icons';
import styled from '@emotion/styled';
import { Button, List, Popconfirm } from 'antd';
import { useTranslation } from 'react-i18next';

import { CategoryReadModel } from '~/types';

const Container = styled(List.Item)`
  border-bottom: 1px solid rgba(0, 0, 0, 0.3);
`;

const AdminTagsItem = ({
  item,
  onDelete,
  onUpdate,
  onShowToggle,
}: {
  item: CategoryReadModel;
  onDelete: (item: CategoryReadModel) => void;
  onUpdate: (item: CategoryReadModel) => void;
  onShowToggle: (item: CategoryReadModel) => void;
}) => {
  const { t } = useTranslation();

  const onDeleteCallback = () => {
    onDelete(item);
  };

  const onUpdateCallback = () => {
    onUpdate(item);
  };

  const handleShowToggle = () => {
    onShowToggle(item);
  };

  return (
    <Container
      actions={[
        <Button key="edit" onClick={onUpdateCallback}>
          Редактировать
        </Button>,
        <Popconfirm
          key="delete"
          placement="left"
          title={`Вы уверены что хотите удалить тег ${item.title}`}
          onConfirm={onDeleteCallback}
          okText="Да"
          cancelText="Нет"
        >
          <Button danger>Удалить</Button>
        </Popconfirm>,
        <Button key="show" onClick={handleShowToggle}>
          {item.showOnMainPage ? <EyeOutlined /> : <EyeInvisibleOutlined />}
        </Button>,
      ]}
    >
      <List.Item.Meta title={item.title} description={t(`enums.type.${item.type}`)} />
    </Container>
  );
};

export default AdminTagsItem;
