import * as React from "react";
import Container from "@mui/material/Container";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import CourseCard from "@/components/CourseCard/CourseCard";
import {useEffect, useState} from "react";
import api from "@/service/api";

export default function StarredPage() {
    const [courses,setCourses] = useState([])
    useEffect(()=>{
        api.get('/api/v1/courses')
            .then(res=> {
                console.log(res.data);
                setCourses(res.data)
            }).catch((e)=>console.log(e))
    },[])
  return (
    <Box sx={{
        display:'grid',
        gridTemplateColumns:'repeat(2,1fr)',
           '@media only screen and (max-width: 1315px)': {
               gridTemplateColumns:'repeat(1,1fr)',
           }
    }}>
        {courses?.map((e)=> <CourseCard
                                id={e?.courseId}
                                lessonsLearned={e?.lessonsLearned}
                                courseName={e.courseName}
                                coursePhoto={e?.coursePhoto}
                                description={e?.description}
                                lastLessonId={e?.lastLessonId}
                                totalLessonsCount={e?.totalLessonsCount}
                                start={e?.courseFree} />) }
    </Box>
  );
}
