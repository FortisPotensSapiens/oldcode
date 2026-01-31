import React, { useEffect, useState } from 'react';
import { Box, Typography } from "@mui/material";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import { styles } from './style';
import Image from "next/image";
import cardImage from '../../assets/images/cardImage.png';
import Button from "@mui/material/Button";
import api from "@/service/api";
import { isImageFileExtensionValid } from '@/helpers/imageValidator';
import { useRouter } from 'next/router';
const settings = {
    dots: false,
    arrow: true,
    infinite: false,
    speed: 500,
    slidesToShow: 2,
    slidesToScroll: 1,
    responsive: [
        {
            breakpoint: 600,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1,
                dots: false,
                arrow: true,
                infinite: false,
                speed: 500,
            }
        },
    ]
};
type Props = {
    id: string,
    courseName: string,
    coursePhoto: string,
    start: boolean,
    lastLessonId: string,
    lessonsLearned: number,
    description: string,
    totalLessonsCount: number
}
const Card: React.FC<Props> = ({ id, courseName, coursePhoto, lessonsLearned, totalLessonsCount }) => {
    const router = useRouter();
    return (
        <Box sx={styles.card}>
            <Image src={isImageFileExtensionValid(coursePhoto) ? `data:image/png;base64,${coursePhoto}` : cardImage}
                alt='Image'
                width={250}
                height={250} />
            <Box sx={styles.cardInfo}>
                <Typography sx={styles.cardTitle}>{courseName}</Typography>
                <Button
                    onClick={e => totalLessonsCount !== 0 ? router.push(`/courses/${id}`) : router.push('#0')}
                    sx={styles.cardButton}
                    className={lessonsLearned === 0 ? "start" : "continues"}
                >
                    {lessonsLearned === 0 ? "Начать" : "Продолжить"}
                </Button>
            </Box>
        </Box>
    )
}
const CoursesSlider = () => {
    const [courses, setCourses] = useState([])
    useEffect(() => {
        api.get('/api/v1/courses')
            .then(res => {
                console.log(res.data);
                setCourses(res.data)
            }).catch(e => console.log(e))
    }, [])
    return (
        <Box sx={styles.box}>
            <Box>
                <Typography sx={styles.sectionTitle}>Моё обучение</Typography>
            </Box>
            <Box sx={styles.sliderSection}>
                <Slider {...settings}>
                    {courses.map(e => {
                        return <Card id={e?.courseId}
                            coursePhoto={e?.coursePhoto}
                            totalLessonsCount={e?.totalLessonsCount}
                            lessonsLearned={e?.lessonsLearned}
                            courseName={e?.courseName} />
                    })}
                </Slider>
            </Box>
        </Box>
    );
};

export default CoursesSlider;