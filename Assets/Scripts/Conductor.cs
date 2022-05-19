using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public AudioSource songPlayer;
    public AudioSource hitSoundPlayer;
    public AudioClip hitSound;

    Chart chart;
    Parser parser;

    public float startYPos;
    public float endYPos;

    // �뷡�� BPM
    public float songBpm;

    // ���� �뷡�� ��� ��ġ
    public float songPosition;

    // �� ���ڿ� �ҿ�Ǵ� �ð����� (60f / BPM)�� ����
    public float secondsPerBeat;

    // Ư�� �뷡���� ���� �κп� �ణ�� ������ �ֱ� ������ �뷡 ��ġ�� ����� �� �� ���鸸ŭ ���־�� ��
    public float songOffset;

    // �뷡 ����� ���۵� ������ �����Ͽ� songPosition�� ����� �� ���־�� ��
    public float dspTimeSong;

    // ��Ʈ ���� �������� ��Ʈ �ı� �������� ǥ�õ� �� �ִ� �ִ� ���� �� (��ũ�� �ӵ�)
    public float beatsShownOnScreen;

    // �� Ʈ�� ���� ������ �����ؾ� �� ��Ʈ�� �迭 ��ġ�� ���
    int trackIndex1 = 0;
    int trackIndex2 = 0;
    int trackIndex3 = 0;
    int trackIndex4 = 0;

    bool isSongStarted = false;

    // ������Ʈ Ǯ���� �̿��� ����ȭ�� ����
    //private Queue<Note> objectPool;

    void Start()
    {
        chart = FindObjectOfType<Chart>().GetComponent<Chart>();
        parser = FindObjectOfType<Parser>().GetComponent<Parser>();
        beatsShownOnScreen = 2f;
        hitSound = hitSoundPlayer.clip;
    }

    void Update()
    {
        // ��Ʈ ������ �Ľ��� �� �ɶ����� ���
        if (parser.isParsed)
        {
            songBpm = chart.bpm * songPlayer.pitch;
            secondsPerBeat = 60f / songBpm;
        }
        else
        {
            return;
        }

        // �����̽��ٸ� ������ ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isSongStarted)
            {
                isSongStarted = true;
                dspTimeSong = (float)AudioSettings.dspTime;
                songPlayer.Play();
                return;
            }
        }

        if (!isSongStarted) return;

        songPosition = (float)(AudioSettings.dspTime - dspTimeSong) * songPlayer.pitch - songOffset;

        float noteToSpawn = songPosition / secondsPerBeat + beatsShownOnScreen;

        if (trackIndex1 < chart.track1.Count)
        {
            float nextTimeInTrack1 = chart.track1[trackIndex1] / 1000 / secondsPerBeat;

            if (nextTimeInTrack1 < noteToSpawn)
            {
                Note note = ObjectPool.GetObject();
                // Note note = ((GameObject)Instantiate(notePrefab, Vector2.zero, Quaternion.identity)).GetComponent<Note>();
                note.Initialize(this, -1.5f, startYPos, endYPos, nextTimeInTrack1);
                trackIndex1++;
            }
        }

        if (trackIndex2 < chart.track2.Count)
        {
            float nextTimeInTrack2 = chart.track2[trackIndex2] / 1000 / secondsPerBeat;

            if (nextTimeInTrack2 < noteToSpawn)
            {
                Note note = ObjectPool.GetObject();
                // Note note = ((GameObject)Instantiate(notePrefab, Vector2.zero, Quaternion.identity)).GetComponent<Note>();
                note.Initialize(this, -0.5f, startYPos, endYPos, nextTimeInTrack2);
                trackIndex2++;
            }
        }

        if (trackIndex3 < chart.track3.Count)
        {
            float nextTimeInTrack3 = chart.track3[trackIndex3] / 1000 / secondsPerBeat;

            if (nextTimeInTrack3 < noteToSpawn)
            {
                Note note = ObjectPool.GetObject();
                // Note note = ((GameObject)Instantiate(notePrefab, Vector2.zero, Quaternion.identity)).GetComponent<Note>();
                note.Initialize(this, 0.5f, startYPos, endYPos, nextTimeInTrack3);
                trackIndex3++;
            }
        }

        if (trackIndex4 < chart.track4.Count)
        {
            float nextTimeInTrack4 = chart.track4[trackIndex4] / 1000 / secondsPerBeat;

            if (nextTimeInTrack4 < noteToSpawn)
            {
                Note note = ObjectPool.GetObject();
                // Note note = ((GameObject)Instantiate(notePrefab, Vector2.zero, Quaternion.identity)).GetComponent<Note>();
                note.Initialize(this, 1.5f, startYPos, endYPos, nextTimeInTrack4);
                trackIndex4++;
            }
        }
    }
}