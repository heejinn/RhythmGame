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
    Judgement judgement;

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

    bool isSongStarted = false;

    void Start()
    {
        chart = FindObjectOfType<Chart>().GetComponent<Chart>();
        parser = FindObjectOfType<Parser>().GetComponent<Parser>();
        judgement = FindObjectOfType<Judgement>().GetComponent<Judgement>();
        beatsShownOnScreen = 1.8f;
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

        if (chart.track1_TimingData.Count > 0)
        {
            float nextTimeInTrack1 = chart.track1_TimingData.Peek() / 1000 / secondsPerBeat;

            if (nextTimeInTrack1 < noteToSpawn)
            {
                Note note = ObjectPool.GetObject();
                note.Initialize(this, -2.25f, startYPos, endYPos, chart.track1_TimingData.Dequeue(), nextTimeInTrack1);
                judgement.EnqueueNote(1, note);
            }
        }

        if (chart.track2_TimingData.Count > 0)
        {
            float nextTimeInTrack2 = chart.track2_TimingData.Peek() / 1000 / secondsPerBeat;

            if (nextTimeInTrack2 < noteToSpawn)
            {
                Note note = ObjectPool.GetObject();
                note.Initialize(this, -0.75f, startYPos, endYPos, chart.track2_TimingData.Dequeue(), nextTimeInTrack2);
                judgement.EnqueueNote(2, note);
            }
        }

        if (chart.track3_TimingData.Count > 0)
        {
            float nextTimeInTrack3 = chart.track3_TimingData.Peek() / 1000 / secondsPerBeat;

            if (nextTimeInTrack3 < noteToSpawn)
            {
                Note note = ObjectPool.GetObject();
                note.Initialize(this, 0.75f, startYPos, endYPos, chart.track3_TimingData.Dequeue(), nextTimeInTrack3);
                judgement.EnqueueNote(3, note);
            }
        }

        if (chart.track4_TimingData.Count > 0)
        {
            float nextTimeInTrack4 = chart.track4_TimingData.Peek() / 1000 / secondsPerBeat;

            if (nextTimeInTrack4 < noteToSpawn)
            {
                Note note = ObjectPool.GetObject();
                note.Initialize(this, 2.25f, startYPos, endYPos, chart.track4_TimingData.Dequeue(), nextTimeInTrack4);
                judgement.EnqueueNote(4, note);
            }
        }
    }
}